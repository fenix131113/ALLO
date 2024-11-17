using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LevelGenerationSystem.Data;
using UnityEditor;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace LevelGenerationSystem
{
    public class LevelGeneration : IInitializable
    {
        private GenerationSettingsSO _generationSettings;

        private readonly Dictionary<Vector2, LevelSegment> _grid = new();
        private float _nextSpawnXPosition;
        private LevelSegmentSO _lastSpawnedSegment;

        [Inject]
        private void Construct(GenerationSettingsSO generationSettings)
        {
            _generationSettings = generationSettings;
        }

        public void Initialize()
        {
            GenerateLevelSegments();
        }

        private void GenerateLevelSegments()
        {
            CreateLevelSegment(_generationSettings.StartSegment, Vector3.zero, Vector2.zero);

            //TODO: Fix randomize
            for (var x = 1; x < _generationSettings.StartSegmentsXCount + 1; x++)
            {
                for (var y = -(_generationSettings.StartSegmentsYCount - 1) / 2;
                     y < (_generationSettings.StartSegmentsYCount - 1) / 2 + 1;
                     y++)
                {
                    CreateLevelSegment(_lastSpawnedSegment.GetRandomNextLevelSegment(),
                        new Vector3(_lastSpawnedSegment.SegmentPrefab.GetWidth() * x,
                            _lastSpawnedSegment.SegmentPrefab.GetHeight() * y, 0),
                        new Vector2(x, y));
                }
            }

            GenerateSegmentsDoors();
            GenerateExit();
        }

        private void GenerateSegmentsDoors()
        {
            foreach (var segment in _grid)
            {
                var connected = GetConnectedSegments(segment.Key);

                switch (connected.Count)
                {
                    case 1:
                    {
                        var element = connected.ElementAt(0);
                        element.Value.SetDoorState(element.Key, false);
                        segment.Value.SetDoorState(InvertDirection(element.Key), false);
                        break;
                    }

                    case > 0:
                    {
                        var usedConnections = new Dictionary<DoorDirection, LevelSegment>();

                        for (var i = 0; i < Random.Range(1, connected.Count); i++)
                        {
                            var unusedConnections = connected.Except(usedConnections);
                            var selected = unusedConnections.ElementAt(Random.Range(0, unusedConnections.Count()));

                            selected.Value.SetDoorState(selected.Key, false);
                            segment.Value.SetDoorState(InvertDirection(selected.Key), false);
                            usedConnections.Add(selected.Key, selected.Value);
                        }

                        break;
                    }
                }
            }

            CheckUnavailableRooms();
        }

        private void CheckUnavailableRooms()
        {
            var availableRooms = _grid[Vector2.zero].GetAllConnectedSegments(_grid);

            var unavailableRooms = _grid.Except(availableRooms);

            var unavailableRoomsLines = new List<Dictionary<Vector2, LevelSegment>>();

            //Connect unavailable rooms
            while (unavailableRooms.Any())
            {
                unavailableRoomsLines.Add(unavailableRooms.ElementAt(0).Value.GetAllConnectedSegments(_grid));

                unavailableRooms = unavailableRooms.Except(unavailableRoomsLines[^1]);


                var roomCountToOpen = Random.Range(0, unavailableRoomsLines[^1].Count / 2);
                var tempUnavailableRooms = new Dictionary<Vector2, LevelSegment>();
                for (var i = 0; i < roomCountToOpen; i++)
                {
                    var excepted = unavailableRoomsLines[^1].Except(tempUnavailableRooms)
                        .Where(pair => !pair.Value.IsAllSidesBusy(_grid))
                        .ToDictionary(e => e.Key, e => e.Value);

                    if (excepted.Count == 0)
                        continue;

                    var selected = excepted.ElementAt(Random.Range(0, excepted.Count));
                    tempUnavailableRooms.Add(selected.Key, selected.Value);

                    var unconnected = selected.Value.GetUnconnectedLocalSegments(_grid);

                    if (unconnected.Count == 0)
                        continue;

                    var neighbour = unconnected[Random.Range(0, unconnected.Count)];

                    var directionToNeighbour = GetDirectionBySecondRoom(selected.Key, neighbour);

                    selected.Value.SetDoorState(directionToNeighbour, false);
                    _grid[neighbour].SetDoorState(InvertDirection(directionToNeighbour), false);
                }
            }
        }

        private void GenerateExit()
        {
            var exitData = (Vector3.zero, Vector2.zero);

            var gridPos = Vector2.zero;
            var spawnPos = Vector3.zero;

            var exitDoorDirection = DoorDirection.UP;

            switch (Random.Range(0, 3))
            {
                case 0:
                    gridPos = new Vector2(
                        _generationSettings.StartSegmentsXCount - (_generationSettings.StartSegmentsXCount - 1) / 2,
                        (_generationSettings.StartSegmentsYCount - 1) / 2 + 1);
                    spawnPos = _grid[gridPos + GetGridVectorDirectionByDoorDirection(DoorDirection.DOWN)].transform
                            .position + Vector3.up *
                        (_grid[gridPos + GetGridVectorDirectionByDoorDirection(DoorDirection.DOWN)].GetHeight() +
                         _generationSettings.EndSegment.SegmentPrefab.GetHeight()) / 2;
                    exitDoorDirection = DoorDirection.DOWN;
                    exitData = (spawnPos, gridPos);
                    break;

                case 1:
                    gridPos = new Vector2(_generationSettings.StartSegmentsXCount + 1, 0);
                    spawnPos = _grid[gridPos + GetGridVectorDirectionByDoorDirection(DoorDirection.LEFT)].transform
                            .position + Vector3.right *
                        (_grid[gridPos + GetGridVectorDirectionByDoorDirection(DoorDirection.LEFT)].GetWidth() +
                         _generationSettings.EndSegment.SegmentPrefab.GetWidth()) / 2;
                    exitDoorDirection = DoorDirection.LEFT;
                    exitData = (spawnPos, gridPos);
                    break;

                case 2:
                    gridPos = new Vector2(
                        _generationSettings.StartSegmentsXCount - (_generationSettings.StartSegmentsXCount - 1) / 2,
                        -(_generationSettings.StartSegmentsYCount - 1) / 2 - 1);
                    spawnPos = _grid[gridPos + GetGridVectorDirectionByDoorDirection(DoorDirection.UP)].transform
                            .position + Vector3.down *
                        (_grid[gridPos + GetGridVectorDirectionByDoorDirection(DoorDirection.UP)].GetHeight() +
                         _generationSettings.EndSegment.SegmentPrefab.GetHeight()) / 2;
                    exitDoorDirection = DoorDirection.UP;
                    exitData = (spawnPos, gridPos);
                    break;
            }

            CreateLevelSegment(_generationSettings.EndSegment, exitData.Item1, exitData.Item2);
            _grid[exitData.Item2].SetDoorState(exitDoorDirection, false);
            _grid[exitData.Item2 + GetGridVectorDirectionByDoorDirection(exitDoorDirection)]
                .SetDoorState(InvertDirection(exitDoorDirection), false);
        }

        /// <returns> Door direction from first room to second</returns>
        private DoorDirection GetDirectionBySecondRoom(Vector2 firstCoords, Vector2 secondCoords)
        {
            if (secondCoords - firstCoords == Vector2.up)
                return DoorDirection.UP;
            if (secondCoords - firstCoords == Vector2.right)
                return DoorDirection.RIGHT;
            if (secondCoords - firstCoords == Vector2.down)
                return DoorDirection.DOWN;
            if (secondCoords - firstCoords == Vector2.left)
                return DoorDirection.LEFT;

            throw new ArgumentException(
                $"Invalid second or first room coordinates! First: {firstCoords} | Second {secondCoords}");
        }

        private Vector2 GetGridVectorDirectionByDoorDirection(DoorDirection doorDirection)
        {
            return doorDirection switch
            {
                DoorDirection.UP => Vector2.up,
                DoorDirection.RIGHT => Vector2.right,
                DoorDirection.DOWN => Vector2.down,
                DoorDirection.LEFT => Vector2.left,
                _ => throw new ArgumentOutOfRangeException(nameof(doorDirection), doorDirection, null)
            };
        }

        private Dictionary<DoorDirection, LevelSegment> GetConnectedSegments(Vector2 coordinates)
        {
            if (!_grid.ContainsKey(coordinates))
                throw new ArgumentException($"Invalid coordinates: ({coordinates.x}, {coordinates.y})!");

            var segments = new Dictionary<DoorDirection, LevelSegment>();

            if (_grid.ContainsKey(coordinates + Vector2.up) &&
                _grid[coordinates + Vector2.up].IsDoorActive(DoorDirection.UP))
                segments.Add(DoorDirection.DOWN, _grid[coordinates + Vector2.up]);

            if (_grid.ContainsKey(coordinates + Vector2.right) &&
                _grid[coordinates + Vector2.right].IsDoorActive(DoorDirection.RIGHT))
                segments.Add(DoorDirection.LEFT, _grid[coordinates + Vector2.right]);

            if (_grid.ContainsKey(coordinates + Vector2.down) &&
                _grid[coordinates + Vector2.down].IsDoorActive(DoorDirection.DOWN))
                segments.Add(DoorDirection.UP, _grid[coordinates + Vector2.down]);

            if (_grid.ContainsKey(coordinates + Vector2.left) &&
                _grid[coordinates + Vector2.left].IsDoorActive(DoorDirection.LEFT))
                segments.Add(DoorDirection.RIGHT, _grid[coordinates + Vector2.left]);

            return segments;
        }

        private static DoorDirection InvertDirection(DoorDirection direction)
        {
            return direction switch
            {
                DoorDirection.UP => DoorDirection.DOWN,
                DoorDirection.DOWN => DoorDirection.UP,
                DoorDirection.LEFT => DoorDirection.RIGHT,
                DoorDirection.RIGHT => DoorDirection.LEFT,
                _ => throw new ArgumentException($"Invalid direction: ({direction})!"),
            };
        }

        private void CreateLevelSegment(LevelSegmentSO so, Vector3 position, Vector2 gridPosition)
        {
            var spawned = Object.Instantiate(so.SegmentPrefab, position, Quaternion.identity).Init(so, gridPosition);

            _lastSpawnedSegment = so;

            spawned.name = spawned.name.Split(' ')[0].Replace("(Clone)", "") + $" ({gridPosition.x}, {gridPosition.y})";
            ;

            if (!_grid.TryAdd(gridPosition, spawned))
                throw new ArgumentException(
                    $"Segment on this grid's coordinates is already exists! ({gridPosition.x}, {gridPosition.y}) ");
        }
    }
}
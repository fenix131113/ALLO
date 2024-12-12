using System;
using System.Collections.Generic;
using System.Linq;
using LevelGenerationSystem.Data;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

// ReSharper disable PossibleLossOfFraction

namespace LevelGenerationSystem
{
    public class LevelGeneration : IInitializable
    {
        private GenerationSettingsSO _generationSettings;

        private readonly Dictionary<Vector2, LevelSegment> _grid = new();
        private LevelSpritesColor _selectedLevelColor;
        private float _nextSpawnXPosition;

        [Inject]
        private void Construct(GenerationSettingsSO generationSettings) => _generationSettings = generationSettings;

        public void Initialize() => GenerateLevel();

        private void GenerateLevel()
        {
            SelectRandomLevelColor();
            CreateLevelSegment(_generationSettings.StartSegment, Vector3.zero, Vector2.zero);

            GenerateLevelSegments();
            GenerateExit();
            GenerateCorridors();
            RandomizeSegments();
        }

        private void SelectRandomLevelColor()
        {
            _selectedLevelColor = Random.Range(0, 4) switch
            {
                0 => LevelSpritesColor.WHITE,
                1 => LevelSpritesColor.RED,
                2 => LevelSpritesColor.GRAY,
                3 => LevelSpritesColor.YELLOW,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void GenerateLevelSegments()
        {
            var spawnedCount = 0;
            var spawnedPath = new List<Vector2>();
            var currentPosition = Vector2.right;
            var previousSegment = _generationSettings.StartSegment.GetRandomNextLevelSegment();
            var excludeCoords = new List<Vector2>();

            // Create first room
            var spawned = CreateLevelSegment(previousSegment,
                currentPosition * previousSegment.SegmentPrefab.GetWidth(), currentPosition);
            excludeCoords.Add(Vector2.zero);
            spawned.SetDoorState(DoorDirection.LEFT, false);
            _grid[Vector2.zero].SetDoorState(DoorDirection.RIGHT, false);
            OnSegmentSpawned();


            // Generate other rooms
            while (spawnedCount < _generationSettings.StartSegmentsXCount * _generationSettings.StartSegmentsYCount)
            {
                // Exclude already checked way
                var unconnected =
                    spawned.GetUnconnectedLocalSegments(_generationSettings).Except(excludeCoords).Except(_grid.Keys)
                        .ToList();

                if (unconnected.Any())
                {
                    var connectIndex = Random.Range(0, unconnected.Count);
                    currentPosition = unconnected[connectIndex];

                    var directionToSpawned = GetDirectionBySecondRoom(spawned.Coordinates, currentPosition);

                    // Open door in last segment
                    spawned.SetDoorState(directionToSpawned, false);

                    spawned = CreateLevelSegment(previousSegment.GetRandomNextLevelSegment(),
                        currentPosition * previousSegment.SegmentPrefab.GetWidth(),
                        currentPosition);

                    OnSegmentSpawned();

                    // Open door in new segment
                    spawned.SetDoorState(InvertDirection(directionToSpawned), false);

                    excludeCoords.Clear();
                }
                else
                {
                    // Return back if unconnected directions is empty
                    excludeCoords.Add(spawned.Coordinates);
                    spawnedPath.RemoveAt(spawnedPath.Count - 1);
                    currentPosition = spawnedPath[^1];
                    spawned = _grid[currentPosition];
                }
            }

            return;

            void OnSegmentSpawned()
            {
                previousSegment = spawned.SO;
                spawnedCount++;
                spawnedPath.Add(currentPosition);
            }
        }

        private void GenerateExit()
        {
            var exitData = (Vector3.zero, Vector2.zero);

            Vector2 gridPos;
            Vector3 spawnPos;

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

        private void GenerateCorridors()
        {
            var tempGrid = _grid.ToDictionary(x => x.Key, x => x.Value);

            var corridorsCount = (int)(_generationSettings.StartSegmentsYCount *
                                       _generationSettings.StartSegmentsXCount *
                                       (_generationSettings.CorridorPercent / 100f));

            while (corridorsCount > 0)
            {
                var selected = tempGrid.ElementAt(Random.Range(0, tempGrid.Count));

                var connected = selected.Value.GetLocalConnectedSegments();

                tempGrid.Remove(selected.Key);

                if (connected.Count is 0 or 1) continue;

                _grid.Remove(selected.Key);

                var doorsList = connected
                    .Select(door => GetDoorDirectionByGridVectorDirection(door - selected.Value.Coordinates)).ToList();

                var corridor = GetCorridorDataByConnected(doorsList);

                CreateLevelSegment(corridor,
                    new Vector3(selected.Key.x * selected.Value.GetWidth(),
                        selected.Key.y * selected.Value.GetHeight(), 0), selected.Key);

                Object.Destroy(selected.Value.gameObject);

                corridorsCount--;
            }
        }

        private void RandomizeSegments()
        {
            foreach (var segmentPair in _grid.Where(segmentPair => segmentPair.Value.RandomSegment))
            {
                segmentPair.Value.RandomSegment.Generate(segmentPair.Value.RandomSegment.NonDoorsGorup);

                var unconnected = segmentPair.Value.GetUnconnectedLocalSegmentsWithNonExist(_generationSettings);
                segmentPair.Value.RandomSegment.Generate(segmentPair.Value.RandomSegment.NonDoorsGorup);

                foreach (var cell in unconnected)
                    switch (GetDirectionBySecondRoom(segmentPair.Key, cell))
                    {
                        case DoorDirection.UP:
                            segmentPair.Value.RandomSegment?.Generate(segmentPair.Value.RandomSegment.UpRandomGroup);
                            break;
                        case DoorDirection.RIGHT:
                            segmentPair.Value.RandomSegment?.Generate(segmentPair.Value.RandomSegment.RightRandomGroup);
                            break;
                        case DoorDirection.DOWN:
                            segmentPair.Value.RandomSegment?.Generate(segmentPair.Value.RandomSegment.DownRandomGroup);
                            break;
                        case DoorDirection.LEFT:
                            segmentPair.Value.RandomSegment?.Generate(segmentPair.Value.RandomSegment.LeftRandomGroup);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
            }
        }

        private LevelSegmentSO GetCorridorDataByConnected(List<DoorDirection> connected)
        {
            switch (connected.Count)
            {
                case 2:
                    if (connected.Contains(DoorDirection.RIGHT) && connected.Contains(DoorDirection.DOWN))
                        return _generationSettings.CornerCorridorSegment;
                    if (connected.Contains(DoorDirection.DOWN) && connected.Contains(DoorDirection.LEFT))
                        return _generationSettings.CornerCorridorSegment90;
                    if (connected.Contains(DoorDirection.LEFT) && connected.Contains(DoorDirection.UP))
                        return _generationSettings.CornerCorridorSegment180;
                    if (connected.Contains(DoorDirection.UP) && connected.Contains(DoorDirection.RIGHT))
                        return _generationSettings.CornerCorridorSegment270;
                    if (connected.Contains(DoorDirection.RIGHT) && connected.Contains(DoorDirection.LEFT))
                        return _generationSettings.LineCorridorSegment;
                    if (connected.Contains(DoorDirection.UP) && connected.Contains(DoorDirection.DOWN))
                        return _generationSettings.LineCorridorSegment90;
                    break;

                case 3:
                    if (connected.Contains(DoorDirection.UP) && connected.Contains(DoorDirection.RIGHT) &&
                        connected.Contains(DoorDirection.DOWN))
                        return _generationSettings.TCorridorSegment;
                    if (connected.Contains(DoorDirection.RIGHT) && connected.Contains(DoorDirection.DOWN) &&
                        connected.Contains(DoorDirection.LEFT))
                        return _generationSettings.TCorridorSegment90;
                    if (connected.Contains(DoorDirection.DOWN) && connected.Contains(DoorDirection.LEFT) &&
                        connected.Contains(DoorDirection.UP))
                        return _generationSettings.TCorridorSegment180;
                    if (connected.Contains(DoorDirection.LEFT) && connected.Contains(DoorDirection.UP) &&
                        connected.Contains(DoorDirection.RIGHT))
                        return _generationSettings.TCorridorSegment270;
                    break;
                case 4:
                    return _generationSettings.XCorridorSegment;
            }

            throw new ArgumentException(
                $"Invalid corridor connections, connections count: {connected.Count}, need 2 - 4!");
        }

        /// <returns> Door direction from first room to second</returns>
        private static DoorDirection GetDirectionBySecondRoom(Vector2 firstCoords, Vector2 secondCoords)
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

        private static Vector2 GetGridVectorDirectionByDoorDirection(DoorDirection doorDirection)
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

        private static DoorDirection GetDoorDirectionByGridVectorDirection(Vector2 gridDirection)
        {
            return gridDirection switch
            {
                var v when v.Equals(Vector2.up) => DoorDirection.UP,
                var v when v.Equals(Vector2.right) => DoorDirection.RIGHT,
                var v when v.Equals(Vector2.down) => DoorDirection.DOWN,
                var v when v.Equals(Vector2.left) => DoorDirection.LEFT,
                _ => throw new ArgumentOutOfRangeException(nameof(gridDirection), gridDirection, null)
            };
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

        private LevelSegment CreateLevelSegment(LevelSegmentSO so, Vector3 position, Vector2 gridPosition,
            int rotation = 0)
        {
            var spawned = Object.Instantiate(so.SegmentPrefab, position, Quaternion.Euler(0, 0, rotation))
                .Init(so, gridPosition);
            spawned.SegmentColorManager?.SelectColor(_selectedLevelColor);
            spawned.name = spawned.name.Split(' ')[0].Replace("(Clone)", "") + $" ({gridPosition.x}, {gridPosition.y})";

            if (!_grid.TryAdd(gridPosition, spawned))
                throw new ArgumentException(
                    $"Segment on this grid's coordinates is already exists! ({gridPosition.x}, {gridPosition.y}) ");

            return spawned;
        }
    }
}
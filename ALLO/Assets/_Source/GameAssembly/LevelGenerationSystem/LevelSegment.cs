using System;
using System.Collections.Generic;
using System.Linq;
using EnemySystem;
using LevelGenerationSystem.Data;
using UnityEngine;
using Zenject;

// ReSharper disable PossibleLossOfFraction

namespace LevelGenerationSystem
{
    public class LevelSegment : MonoBehaviour
    {
        [Tooltip("Generator will use this object to calculate next room position by it's size")] [SerializeField]
        private Transform backGround;

        [field: SerializeField] public RandomSegment RandomSegment { get; set; } 
        [field: SerializeField] public SegmentColorManager SegmentColorManager { get; set; } 
        [SerializeField] private GameObject upDoor;
        [SerializeField] private GameObject rightDoor;
        [SerializeField] private GameObject downDoor;
        [SerializeField] private GameObject leftDoor;

        public LevelSegmentSO SO { get; private set; }
        public Vector2 Coordinates { get; private set; }

        private DiContainer _diContainer;


        public LevelSegment Init(LevelSegmentSO so, Vector2 coordinates, DiContainer diContainer)
        {
            if (SO)
                return null;

            SO = so;
            Coordinates = coordinates;
            _diContainer = diContainer;
            
            var enemies = transform.GetComponentsInChildren<AEnemy>();
            foreach (var enemy in enemies)
                _diContainer.InjectGameObject(enemy.gameObject);
            
            return this;
        }

        public void SetDoorState(DoorDirection direction, bool state)
        {
            var selectedDoor = direction switch
            {
                DoorDirection.UP => upDoor,
                DoorDirection.RIGHT => rightDoor,
                DoorDirection.DOWN => downDoor,
                DoorDirection.LEFT => leftDoor,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction,
                    "This is not a valid direction.")
            };

            selectedDoor.SetActive(state);
        }

        public bool IsDoorActive(DoorDirection direction)
        {
            if (!upDoor || !rightDoor || !downDoor || !leftDoor)
                return true;

            return direction switch
            {
                DoorDirection.UP => upDoor.activeSelf,
                DoorDirection.RIGHT => rightDoor.activeSelf,
                DoorDirection.DOWN => downDoor.activeSelf,
                DoorDirection.LEFT => leftDoor.activeSelf,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction,
                    "This is not a valid direction.")
            };
        }

        /// <returns>Each connected segment in the line</returns>
        public Dictionary<Vector2, LevelSegment> GetAllConnectedSegments(Dictionary<Vector2, LevelSegment> grid)
        {
            var connected = new Dictionary<Vector2, LevelSegment> { { Coordinates, this } };

            foreach (var segmentCoordinate in GetLocalConnectedSegments())
                grid[segmentCoordinate].GetAllConnectedSegments(grid, connected);

            return connected;
        }

        private void GetAllConnectedSegments(Dictionary<Vector2, LevelSegment> grid,
            Dictionary<Vector2, LevelSegment> connectedSegments)
        {
            var connected = connectedSegments;

            if (connected.ContainsKey(Coordinates))
                return;

            connected.Add(Coordinates, this);

            foreach (var segmentCoordinate in GetLocalConnectedSegments()
                         .Where(segmentCoordinate => !connected.ContainsKey(segmentCoordinate)))
                grid[segmentCoordinate].GetAllConnectedSegments(grid, connected);
        }

        public List<Vector2> GetUnconnectedLocalSegments(int xSize, int ySize)
        {
            var connected = GetLocalConnectedSegments();

            if (connected.Count == 0)
                return new List<Vector2>();

            var emptySides = new List<Vector2>
            {
                Coordinates + Vector2.up,
                Coordinates + Vector2.right,
                Coordinates + Vector2.down,
                Coordinates + Vector2.left
            };

            emptySides = emptySides.Except(connected).ToList();

            var result = emptySides.Where(coords =>
                    coords.x <= xSize
                    && coords.x >= 1
                    && coords.y >= (ySize - 1) / -2
                    && coords.y <= (ySize - 1) / 2)
                .ToList();
            
            return result;
        }
        
        public List<Vector2> GetUnconnectedLocalSegmentsWithNonExist(GenerationSettingsSO settings)
        {
            var connected = GetLocalConnectedSegments();

            if (connected.Count == 0)
                return new List<Vector2>();

            var emptySides = new List<Vector2>
            {
                Coordinates + Vector2.up,
                Coordinates + Vector2.right,
                Coordinates + Vector2.down,
                Coordinates + Vector2.left
            };

            emptySides = emptySides.Except(connected).ToList();
            
            return emptySides;
        }

        public bool IsAllSidesBusy(Dictionary<Vector2, LevelSegment> grid)
        {
            var connected = GetLocalConnectedSegments();

            if (connected.Count == 0)
                return false;

            var emptySides = new List<Vector2>
            {
                Coordinates + Vector2.up,
                Coordinates + Vector2.right,
                Coordinates + Vector2.down,
                Coordinates + Vector2.left
            };

            emptySides = emptySides.Except(connected).ToList();

            return emptySides.All(side => !grid.ContainsKey(side));
        }

        public List<Vector2> GetLocalConnectedSegments()
        {
            var connected = new List<Vector2>();

            if (!IsDoorActive(DoorDirection.UP))
                connected.Add(Coordinates + Vector2.up);

            if (!IsDoorActive(DoorDirection.RIGHT))
                connected.Add(Coordinates + Vector2.right);

            if (!IsDoorActive(DoorDirection.DOWN))
                connected.Add(Coordinates + Vector2.down);

            if (!IsDoorActive(DoorDirection.LEFT))
                connected.Add(Coordinates + Vector2.left);

            return connected;
        }

        public float GetWidth()
        {
            if (backGround.TryGetComponent(out SpriteRenderer spriteRenderer) &&
                spriteRenderer.drawMode == SpriteDrawMode.Tiled)
                return spriteRenderer.size.x;

            return backGround.lossyScale.x;
        }

        public float GetHeight()
        {
            if (backGround.TryGetComponent(out SpriteRenderer spriteRenderer) &&
                spriteRenderer.drawMode == SpriteDrawMode.Tiled)
                return spriteRenderer.size.y;

            return backGround.lossyScale.y;
        }
    }
}
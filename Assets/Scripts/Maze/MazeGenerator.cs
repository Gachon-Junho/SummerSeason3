using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
    {
        public Vector2Int mazeSize = new Vector2Int(25, 25);

        private Vector2Int BlockSize => mazeSize / 2;

        public Vector2 TopLeft
        {
            get
            {
                var mazeHalfSize = new Vector3(mazeSize.x, mazeSize.y, 0) / 2;
                var wallPosition = new Vector3(1, mazeSize.y - 2, 0) - mazeHalfSize + transform.position;
                
                return wallPosition;
            }
        }
        
        public Vector2 BottomRight
        {
            get
            {
                var mazeHalfSize = new Vector3(mazeSize.x, mazeSize.y, 0) / 2;
                var dest = dfs(Maze, new bool[Maze.GetLength(0), Maze.GetLength(1)], 1, mazeSize.y - 2);
                var wallPosition = new Vector3(dest.x, mazeSize.y - dest.y, 0) - mazeHalfSize + transform.position;
                wallPosition = new Vector3(mazeSize.x - 2, 1, 0) - mazeHalfSize + transform.position;

                return wallPosition;
            }
        }

        private Block[,] _blocks;
        public bool[,] Maze; // 벽이 존재하지 않으면 true
        private DisjointSet _disjointSet;
        private readonly Dictionary<int, List<int>> _lastRowBlocks = new Dictionary<int, List<int>>();

        [SerializeField] private bool isDrawGizmo;
        [SerializeField] private GameObject wallPrefab;

        public List<Vector2Int> GetRoadIndexes()
        {
            List<Vector2Int> indexes = new List<Vector2Int>();
            
            for (int i = 0; i < Maze.GetLength(0); i++)
            {
                for (int j = 0; j < Maze.GetLength(1); j++)
                {
                    if (Maze[i, j])
                        indexes.Add(new Vector2Int(i, j));
                }
            }

            return indexes;
        }

        public void Build(int width, int height)
        {
            mazeSize = new Vector2Int(width, height);
            _blocks = new Block[BlockSize.x, BlockSize.y];
            Maze = new bool[mazeSize.x, mazeSize.y];
            _disjointSet = new DisjointSet(BlockSize.x * BlockSize.y);
            
            InitBlocks();
            
            for (int y = 0; y < BlockSize.y - 1; y++)
            {
                RandomMergeRowBlocks(y);
                DropDownGroups(y);
            }

            OrganizeLastLine();
            MakeHoleInPath();
            BuildWalls();
        }

        /// <summary>
        /// 블럭을 초기화 하는 함수.
        /// A function that initializes blocks.
        /// </summary>
        private void InitBlocks()
        {
            for (int x = 0; x < BlockSize.x; x++)
            {
                for (int y = 0; y < BlockSize.y; y++)
                {
                    _blocks[x, y] = new Block();
                }
            }
        }

        /// <summary>
        /// 행의 블럭을 순차적으로 접근하면서 선택한 블럭과 오른쪽 블럭을 랜덤하게 합치는 함수.
        /// A function of randomly merge the selected block and the right block while sequentially approaching the blocks of the row.
        /// </summary>
        /// <param name="row">현재 행 (current row)</param>
        private void RandomMergeRowBlocks(int row)
        {
            for (int x = 0; x < BlockSize.x - 1; x++)
            {
                var canMerge = Random.Range(0, 2) == 1;
                var currentBlockNumber = _blocks[x, row].BlockNumber;
                var nextBlockNumber = _blocks[x + 1, row].BlockNumber;

                if (canMerge && !_disjointSet.IsUnion(currentBlockNumber, nextBlockNumber))
                {
                    _disjointSet.Merge(currentBlockNumber, nextBlockNumber);
                    _blocks[x, row].OpenWay[(int)Direction.Right] = true;
                }
            }
        }

        /// <summary>
        /// 현재 행에서 가지를 내리는 함수
        /// A function of branching off the current row.
        /// </summary>
        /// <param name="row">현재 행 (current row)</param>
        private void DropDownGroups(int row)
        {
            _lastRowBlocks.Clear();

            for (int x = 0; x < BlockSize.x; x++)
            {
                var blockNumber = _blocks[x, row].BlockNumber;
                var parentNumber = _disjointSet.Find(_blocks[x, row].BlockNumber);

                if (!_lastRowBlocks.ContainsKey(parentNumber))
                {
                    _lastRowBlocks.Add(parentNumber, new List<int>());
                }

                _lastRowBlocks[parentNumber].Add(blockNumber);
            }

            foreach (var group in _lastRowBlocks)
            {
                if (group.Value.Count == 0) continue;

                var randomDownCount = Random.Range(1, group.Value.Count);

                for (int i = 0; i < randomDownCount; i++)
                {
                    var randomBlockIndex = Random.Range(0, group.Value.Count);

                    var currentBlockNumber = group.Value[randomBlockIndex];
                    var currentBlockPosition = Block.GetPosition(currentBlockNumber, BlockSize);

                    var currentBlock = _blocks[currentBlockPosition.x, currentBlockPosition.y];
                    var underBlock = _blocks[currentBlockPosition.x, currentBlockPosition.y + 1];

                    _disjointSet.Merge(currentBlock.BlockNumber, underBlock.BlockNumber);
                    currentBlock.OpenWay[(int)Direction.Down] = true;

                    group.Value.RemoveAt(randomBlockIndex);
                }
            }
        }
        
        /// <summary>
        /// 마지막 줄을 정리하는 함수
        /// A function that organizes the last line.
        /// </summary>
        private void OrganizeLastLine()
        {
            var lastRow = BlockSize.y - 1;

            for (int x = 0; x < BlockSize.x - 1; x++)
            {
                var currentBlock = _blocks[x, lastRow];
                var nextBlock = _blocks[x + 1, lastRow];

                if (!_disjointSet.IsUnion(currentBlock.BlockNumber, nextBlock.BlockNumber))
                {
                    currentBlock.OpenWay[(int)Direction.Right] = true;
                }
            }
        }

        private void MakeHoleInPath()
        {
            for (int x = 0; x < BlockSize.x; x++)
            {
                for (int y = 0; y < BlockSize.y; y++)
                {
                    var adjustPosition = new Vector2Int(x * 2 + 1, y * 2 + 1);
                    Maze[adjustPosition.x, adjustPosition.y] = true;

                    if (_blocks[x, y].OpenWay[(int)Direction.Down])
                        Maze[adjustPosition.x, adjustPosition.y + 1] = true;
                    if (_blocks[x, y].OpenWay[(int)Direction.Right])
                        Maze[adjustPosition.x + 1, adjustPosition.y] = true;
                }
            }
        }

        private void BuildWalls()
        {
            for (int x = 0; x < mazeSize.x; x++)
            {
                for (int y = 0; y < mazeSize.y; y++)
                {
                    if (Maze[x, y]) continue;

                    var myTransform = transform;
                    var mazeHalfSize = new Vector3(mazeSize.x, mazeSize.y, 0) / 2;
                    var wallPosition = new Vector3(x, y, 0) - mazeHalfSize + myTransform.position;

                    Instantiate(wallPrefab, wallPosition, Quaternion.identity, myTransform);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying && isDrawGizmo)
            {
                Gizmos.DrawCube(TopLeft, Vector3.one);
                Gizmos.DrawCube(BottomRight, Vector3.one);
            }
        }
        
        private (int x, int y) dfs(bool[,] maze, bool[,] visited, int x, int y)
        {
            int[] dx = { 1, 0, -1, 0 }; // 남, 동, 북, 서
            int[] dy = { 0, 1, 0, -1 };
            
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);

            // 유효하지 않은 좌표거나 벽이거나 이미 방문한 경우 종료
            if (x < 0 || y < 0 || x >= rows || y >= cols || !maze[x, y] || visited[x, y])
            {
                return (-1, -1);
            }

            // 현재 위치 방문 표시
            visited[x, y] = true;

            // 더 이상 나아갈 수 없는 위치인지 확인
            bool isDeadEnd = true;
            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (nx >= 0 && ny >= 0 && nx < rows && ny < cols && maze[nx, ny] && !visited[nx, ny])
                {
                    isDeadEnd = false;
                    break;
                }
            }

            // 목적지 업데이트: 더 이상 나아갈 수 없는 위치 중 가장 오른쪽 하단의 위치
            (int x, int y) destination = isDeadEnd ? (x, y) : (-1, -1);

            // 4 방향으로 DFS 탐색
            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];
                var nextDestination = dfs(maze, visited, nx, ny);
                if (nextDestination != (-1, -1) && 
                    (destination == (-1, -1) || 
                     nextDestination.x > destination.x || 
                     (nextDestination.x == destination.x && nextDestination.y > destination.y)))
                {
                    destination = nextDestination;
                }
            }

            return destination;
        }
        
        public List<Vector2Int> FindShortestPath(Vector2Int start, Vector2Int goal)
        {
            int[] dx = { 1, 0, -1, 0 }; // 남, 동, 북, 서
            int[] dy = { 0, 1, 0, -1 };
            
            if (!Maze[start.x, start.y] || !Maze[goal.x, goal.y])
                return null;

            bool[,] visited = new bool[mazeSize.x, mazeSize.y];
            Vector2Int[,] parent = new Vector2Int[mazeSize.x, mazeSize.y];
            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            queue.Enqueue(start);
            visited[start.x, start.y] = true;
            parent[start.x, start.y] = new Vector2Int(-1, -1);  // 시작점의 부모는 없음

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();

                if (current == goal)
                {
                    return ConstructPath(parent, goal);
                }

                for (int i = 0; i < 4; i++)
                {
                    int newRow = current.x + dx[i];
                    int newCol = current.y + dy[i];

                    if (IsValid(newRow, newCol) && Maze[newRow, newCol] && !visited[newRow, newCol])
                    {
                        queue.Enqueue(new Vector2Int(newRow, newCol));
                        visited[newRow, newCol] = true;
                        parent[newRow, newCol] = current;
                    }
                }
            }

            return null;
        }

        private bool IsValid(int row, int col)
        {
            return row >= 0 && row < mazeSize.x && col >= 0 && col < mazeSize.y;
        }

        private List<Vector2Int> ConstructPath(Vector2Int[,] parent, Vector2Int goal)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            
            for (Vector2Int at = goal; at.x != -1; at = parent[at.x, at.y])
            {
                path.Add(at);
            }
            
            path.Reverse();
            return path;
        }
    }
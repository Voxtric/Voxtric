using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.MonoBehaviours;

namespace VoxelEngine.Hidden
{
    public sealed class DataSplitFinder
    {
        private IntVec3 _startPosition;
        private List<IntVec3> _found = new List<IntVec3>();
        private List<IntVec3> _confirmed = new List<IntVec3>();
        private List<IntVec3> _newPositions;

        private RegionCollection _regionCollection;
        private List<DataSplitFinder> _finders;
        private List<DataSplitFinder> _findersToRemove;

        public DataSplitFinder(IntVec3 startPosition, RegionCollection regionCollection, List<DataSplitFinder> finders, List<DataSplitFinder> findersToRemove)
        {
            _startPosition = startPosition;
            _regionCollection = regionCollection;
            _finders = finders;
            _findersToRemove = findersToRemove;
            _confirmed.Add(startPosition);
        }

        public bool ContainsPosition(IntVec3 position)
        {
            return /*_found.Contains(position) || */_confirmed.Contains(position);
        }

        private DataSplitFinder FinderFound(IntVec3 position, DataSplitFinder toIgnore)
        {
            foreach (DataSplitFinder finder in _finders)
            {
                if (finder != toIgnore && finder.ContainsPosition(position))
                {
                    return finder;
                }
            }
            return null;
        }

        private void CheckPosition(IntVec3 position)
        {
            if (VoxelEdit.GetAt(_regionCollection, position).visible == 1)
            {
                DataSplitFinder finder = FinderFound(position, this);
                if (!ReferenceEquals(finder, null))
                {
                    MergeLists(finder);
                }
                else if (!ContainsPosition(position))
                {
                    _newPositions.Add(position);
                }
            }
        }

        public void Iterate()
        {
            if (_confirmed.Count == 0)
            {
                _findersToRemove.Add(this);
                Debug.Log("Finder finished.");
            }
            _newPositions = new List<IntVec3>();
            IntVec3 newPosition;
            foreach (IntVec3 position in _confirmed)
            {
                _found.Add(position);
                //CheckPosition(position + IntVec3.right);

                newPosition = position + IntVec3.right;
                if (VoxelEdit.GetAt(_regionCollection, newPosition).visible == 1)
                {
                    DataSplitFinder finder = FinderFound(newPosition, this);
                    if (!ReferenceEquals(finder, null))
                    {
                        MergeLists(finder);
                    }
                    else if (!ContainsPosition(newPosition))
                    {
                        _newPositions.Add(newPosition);
                    }
                }

                newPosition = position + IntVec3.left;
                if (VoxelEdit.GetAt(_regionCollection, newPosition).visible == 1)
                {
                    DataSplitFinder finder = FinderFound(newPosition, this);
                    if (!ReferenceEquals(finder, null))
                    {
                        MergeLists(finder);
                    }
                    else if (!ContainsPosition(newPosition))
                    {
                        _newPositions.Add(newPosition);
                    }
                }

                newPosition = position + IntVec3.forward;
                if (VoxelEdit.GetAt(_regionCollection, newPosition).visible == 1)
                {
                    DataSplitFinder finder = FinderFound(newPosition, this);
                    if (!ReferenceEquals(finder, null))
                    {
                        MergeLists(finder);
                    }
                    else if (!ContainsPosition(newPosition))
                    {
                        _newPositions.Add(newPosition);
                    }
                }

                newPosition = position + IntVec3.back;
                if (VoxelEdit.GetAt(_regionCollection, newPosition).visible == 1)
                {
                    DataSplitFinder finder = FinderFound(newPosition, this);
                    if (!ReferenceEquals(finder, null))
                    {
                        MergeLists(finder);
                    }
                    else if (!ContainsPosition(newPosition))
                    {
                        _newPositions.Add(newPosition);
                    }
                }

                newPosition = position + IntVec3.up;
                if (VoxelEdit.GetAt(_regionCollection, newPosition).visible == 1)
                {
                    DataSplitFinder finder = FinderFound(newPosition, this);
                    if (!ReferenceEquals(finder, null))
                    {
                        MergeLists(finder);
                    }
                    else if (!ContainsPosition(newPosition))
                    {
                        _newPositions.Add(newPosition);
                    }
                }

                newPosition = position + IntVec3.down;
                if (VoxelEdit.GetAt(_regionCollection, newPosition).visible == 1)
                {
                    DataSplitFinder finder = FinderFound(newPosition, this);
                    if (!ReferenceEquals(finder, null))
                    {
                        MergeLists(finder);
                    }
                    else if (!ContainsPosition(newPosition))
                    {
                        _newPositions.Add(newPosition);
                    }
                }
            }
            _confirmed = _newPositions;
        }

        private void MergeLists(DataSplitFinder finder)
        {
            DataSplitInfo info = finder.GetFinderInfo();
            _found.AddRange(info.found);
            _newPositions.AddRange(info.confirmed);
            _findersToRemove.Add(finder);
        }

        public DataSplitInfo GetFinderInfo()
        {
            return new DataSplitInfo(_found, _confirmed);
        }
    }
}
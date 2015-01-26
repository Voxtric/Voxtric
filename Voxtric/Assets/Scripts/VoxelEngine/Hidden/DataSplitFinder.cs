using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.MonoBehaviours;

namespace VoxelEngine.Hidden
{
    public sealed class DataSplitFinder
    {
        private List<IntVec3> _found = new List<IntVec3>();
        private List<IntVec3> _confirmed = new List<IntVec3>();
        private List<IntVec3> _lastConfirmed = new List<IntVec3>();
        private List<IntVec3> _newPositions = new List<IntVec3>();

        private RegionCollection _regionCollection;
        private List<DataSplitFinder> _finders;
        private List<DataSplitFinder> _findersToRemove;

        public DataSplitFinder(IntVec3 startPosition, RegionCollection regionCollection, List<DataSplitFinder> finders, List<DataSplitFinder> findersToRemove)
        {
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
                if (!ReferenceEquals(finder, toIgnore) && !_findersToRemove.Contains(finder) && finder.ContainsPosition(position))
                {
                    return finder;
                }
            }
            return null;
        }

        private void CheckPosition(IntVec3 position)
        {
            if (/*VoxelEdit.ValidPosition(_regionCollection.GetDimensions() * VoxelData.SIZE, position) && */VoxelEdit.GetAt(_regionCollection, position).visible == 1)
            {
                DataSplitFinder finder = FinderFound(position, this);
                if (!ReferenceEquals(finder, null))
                {
                    MergeLists(finder);
                }
                else if (!_lastConfirmed.Contains(position) && !_confirmed.Contains(position) && !_newPositions.Contains(position))
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
                Block[] data = new Block[_found.Count];
                //Debug.Log(string.Format("Finder split {0} voxels.", data.Length));
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = VoxelEdit.GetAt(_regionCollection, _found[i]);
                    VoxelEdit.SetAt(_regionCollection, _found[i], new Block());
                }
                _regionCollection.SetPositionsToSplit(_found.ToArray(), data);
            }
            foreach (IntVec3 position in _confirmed)
            {
                _found.Add(position);
                CheckPosition(position + IntVec3.right);
                CheckPosition(position + IntVec3.left);
                CheckPosition(position + IntVec3.forward);
                CheckPosition(position + IntVec3.back);
                CheckPosition(position + IntVec3.up);
                CheckPosition(position + IntVec3.down);
            }
            _lastConfirmed = _confirmed;
            _confirmed = _newPositions;
            _newPositions = new List<IntVec3>();
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
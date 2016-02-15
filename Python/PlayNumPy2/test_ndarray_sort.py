

import unittest
import numpy as np
import numpy.testing as npt

class NdArraySortTest(unittest.TestCase):

    def test_copy_sort(self):
        original = np.arange(4,0,-1)
        sortedcopy = np.sort(original)
        
        # np.sort return a copy, so the original array is not modified
        npt.assert_equal([4,3,2,1],original)
        npt.assert_equal([1,2,3,4],sortedcopy)

        # sort return a copy, not a view
        sortedcopy *= -1
        npt.assert_equal([-1, -2, -3, -4],sortedcopy)
        npt.assert_equal([4,3,2,1],original)

    def test_inplace_sort(self):
        a = np.arange(4,0,-1)
        a.sort() # return None
        npt.assert_equal([1,2,3,4],a)

    def test_argsort(self):
        a = np.asarray([1,8,4,0,3])
        sorted_indices = a.argsort() # also can be called by np.argsort(a)
        npt.assert_equal([3, 0, 4, 2, 1],sorted_indices)
        npt.assert_equal([0,1,3,4,8],a[sorted_indices])

    def test_sort_along_axis(self):
        a = np.asarray([1,2,7,9,4,5,3,8,6]).reshape((3,3))
        npt.assert_equal([[1, 2, 7],       
                          [9, 4, 5],       
                          [3, 8, 6]],a)

        # from the horizontal direction
        npt.assert_equal([[1, 2, 5],       
                           [3, 4, 6],       
                           [9, 8, 7]], np.sort(a,0))

        # along the vertical direction
        npt.assert_equal([[1, 2, 7],       
                           [4, 5, 9],       
                           [3, 6, 8]], np.sort(a,1))

    def test_searchsorted(self):
        """
        searchsorted is an array method that performs a binary search on a sorted array, 
        returning the location in the array where the value would need to be inserted to maintain sortedness:
        """
        arr = np.asarray([0, 1, 7, 12, 15])
        self.assertEqual(3, arr.searchsorted(9))

        # can pass in multiple items
        npt.assert_equal([0, 3, 3, 5], arr.searchsorted([0, 8, 11, 16]) )
        
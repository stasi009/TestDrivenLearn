
import unittest
import numpy as np
import numpy.testing as npt

class NdArraySetTest(unittest.TestCase):
    """
    set operation based on ndarray
    """
    def test_unique(self):
        # ------------------ one dimension
        a = np.asarray([1,9,1,1,2,4,2,2,9,9,9,3,3,3])
        expected = [1,2,3,4,9] # not only unique but also sorted
        npt.assert_equal(expected,np.unique(a))

        # ------------------ multiple dimension
        m = np.asarray([[1, 1], [2, 3]])
        npt.assert_equal([1, 2, 3], np.unique(m))

        # ------------------ return indices
        a = np.array(['b', 'a', 'a', 'c', 'b'])
        u, indices = np.unique(a, return_index=True)#return the indices of ar that result in the unique array.
        npt.assert_equal(["a","b","c"],u)
        npt.assert_equal([1,0,3],indices)
        npt.assert_equal(u,a[indices])

    def test_in1d(self):
        """
        Test whether each element of a 1-D array is also present in a second array.
        Returns a boolean array the same length as ar1 that is True where an element of ar1 is in ar2 and False otherwise.
        """
        # ------------------------ both input arrays are unique
        to_check = [1,2,3,4,5,6]
        check_against = [2,5]
        # the 3rd argument is True, the input arrays are "both" assumed to be unique, which can speed up the calculation. 
        # Default is False.
        result = np.in1d(to_check,check_against,True)
        npt.assert_equal([False,True,False,False,True,False],result)

        # ------------------------ can have duplicates
        test = np.array([0, 1, 2, 5, 0])
        states = [0, 2]
        npt.assert_equal([True,False,True,False,True], np.in1d(test, states) )

    def test_intersect1d(self):
        # ------------------------ can have duplicates
        # return sorted, common elements
        npt.assert_equal( [1,3],np.intersect1d([1, 3, 4, 3], [3, 1, 2, 1]))

        # ------------------------ if both input arrays have only unique elements
        # set last parameter to be True to speed up
        npt.assert_equal( [2,3], np.intersect1d([3,2,1],[2,3,4],True) )

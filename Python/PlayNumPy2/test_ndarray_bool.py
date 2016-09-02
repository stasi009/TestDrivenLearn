
import unittest
import numpy as np
import numpy.testing as npt

class NdArrayBoolTest(unittest.TestCase):

    def test_where_return_indices(self):
        # ------------------------- 1d array
        a = np.asarray([1,4,2,5,3,6])
        (indices,) = np.where(a%2==0)# where return a tuple
        npt.assert_equal([1,2,5],indices)
        npt.assert_equal([4,2,6],a[indices])

        # ------------------------- 2d array
        a = np.asarray([[1,2,3],
                        [4,5,6]])
        (rows,cols) = np.where(a%2==0)
        npt.assert_equal([0,1,1],rows)
        npt.assert_equal([1,0,2],cols)
        npt.assert_equal([2,4,6],a[rows,cols])

    def test_where_conditional_return(self):
        """
        if true, return element in first array, otherwise, return element from second array
        """
        a = [1,2,3]
        b = [4,5,6]
        npt.assert_equal([1,5,3], np.where([True,False,True],a,b))

    def test_multidim_bool_index(self):
        a = np.asarray( [[1,2,3],
                         [4,5,6],
                         [7,8,9]] )
        b = a%2==0
        npt.assert_equal([[False,  True, False],
                          [ True, False,  True],
                          [False,  True, False]],b)

        npt.assert_equal([2,4,6,8], a[b])# return 1d array

    def test_composite(self):
        a = np.arange(1,5)

        b1 = a > 2
        npt.assert_equal([False,False,True,True],b1)

        b2 = a%2 == 0
        npt.assert_equal([False,True,False,True],b2)

        # composite use operator
        npt.assert_equal([False,False,False,True],b1 & b2)
        npt.assert_equal([False,True,True,True],b1 | b2)
        # -b is deprecated, should use ~b instead
        npt.assert_equal([True,True,False,False],~b1)

        # composite use function
        npt.assert_equal([False,False,False,True],np.logical_and(b1,b2))
        npt.assert_equal([False,True,True,True],np.logical_or(b1,b2))
        npt.assert_equal([True,True,False,False],np.logical_not(b1))

    def test_all(self):
        # all can be used to detect zeros in an array
        a = np.asarray([0.0,1.0,2.0,3.0])
        self.assertFalse(a.all())

        a += 0.001
        self.assertTrue(a.all())

        # or, all can be used on boolean array
        b = np.asarray([[True,True],[True,False]])
        self.assertFalse(b.all())

    def test_any(self):
        # -------------- on boolean array
        b = np.asarray([[True,True],[True,False]])
        self.assertTrue(b.any())

        # -------------- on non-boolean array, where non-zero evaluates to be True
        self.assertFalse(np.zeros((2,3)).any())

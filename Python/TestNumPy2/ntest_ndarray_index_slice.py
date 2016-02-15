
import numpy as np
import numpy.testing as npt
import unittest

class NTest_NdArray_Index_Slice(unittest.TestCase):

    def test_basic_slice_return_view(self):
        original = np.arange(12).reshape(3,4)
        npt.assert_array_equal([[ 0,  1,  2,  3],
                                [ 4,  5,  6,  7],
                                [ 8,  9, 10, 11]],original)

        # a continous range will trigger basic slice
        # which will return a view
        sliced = original[:,1:3]
        npt.assert_array_equal([[ 1,  2],
                                [ 5,  6],
                                [ 9, 10]],sliced)

        # make changes on the view
        sliced[:] = -99

        # changes reflect on the original
        npt.assert_array_equal([[  0, -99, -99,   3],
                                [  4, -99, -99,   7],
                                [  8, -99, -99,  11]],original)


    def test_advanced_slice_return_copy(self):
        """
        not all slice on ndarray will return view
        advanced slice will return copy
        """
        original = np.arange(12).reshape(3,4)
        npt.assert_array_equal([[ 0,  1,  2,  3],
                                [ 4,  5,  6,  7],
                                [ 8,  9, 10, 11]],original)

        # a discrete range will trigger advaned slicing
        # and advanced slicing always return copy
        sliced = original[:,(1,2)]
        npt.assert_array_equal([[ 1,  2],
                                [ 5,  6],
                                [ 9, 10]],sliced)

        # make changes on the copy
        sliced[:] = -99

        # changes NOT reflect on the original
        npt.assert_array_equal([[ 0,  1,  2,  3],
                                [ 4,  5,  6,  7],
                                [ 8,  9, 10, 11]],original)

    def test_bool_slice_return_copy(self):
        """
        slicing by bool index is always 'advanced slicing'
        so it will always return copy
        """
        a = np.arange(6)
        cpy = a[a%2 == 0]
        npt.assert_array_equal([0,2,4],cpy)

        # changes on copy will not affect original
        cpy[0] = -999
        npt.assert_array_equal([-999,2,4],cpy)
        npt.assert_array_equal([0,1,2,3,4,5],a)

    def test_2d_bool_index(self):
        a = np.arange(12).reshape(3,4)

        # !!! pay attention that, the boolean indices must be ndarray
        # !!! cannot be array-like
        rows_wanted = np.asarray([False,True,False])
        cols_wanted = np.asarray([True,True,False,False])

        npt.assert_array_equal( [[ 4,  5,  6,  7]],a[rows_wanted,:] )
        npt.assert_array_equal([[0, 1],
                                [4, 5],
                                [8, 9]],a[:,cols_wanted])
        npt.assert_array_equal([4, 5],a[rows_wanted,cols_wanted])



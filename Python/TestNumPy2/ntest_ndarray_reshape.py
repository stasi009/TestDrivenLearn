
import numpy as np
import numpy.testing as npt
import unittest

class NTest_NdArray_Reshape(unittest.TestCase):

    def test_reshape_rowfirst(self):
        """
        The order of the elements in the array resulting from ravel() is normally "C-style", 
        that is, the rightmost index "changes the fastest", so the element after a[0,0] is a[0,1]

        so when reshape, row goes first
        """
        oriarray = np.arange(12)
        view1 = oriarray.reshape(3,4) # pass a tuple like (3,4) also work
        npt.assert_array_equal([[ 0,  1,  2,  3],
                                [ 4,  5,  6,  7],
                                [ 8,  9, 10, 11]],view1)

        view2 = view1.reshape(6,2)
        npt.assert_array_equal([[ 0,  1],
                                [ 2,  3],
                                [ 4,  5],
                                [ 6,  7],
                                [ 8,  9],
                                [10, 11]],view2)

    def test_ravel(self):
        a = np.arange(6).reshape((3,2))
        npt.assert_array_equal([[0, 1],
                                [2, 3],
                                [4, 5]],a)
        self.assertEqual((3,2),a.shape)

        # ravel to return a one-dimensional array
        onedim_array = a.ravel()
        self.assertEqual(1,onedim_array.ndim)
        self.assertEqual((6,),onedim_array.shape)
        npt.assert_array_equal(range(6),onedim_array)

        # ravel returns a dynamic view
        # so change in raveled array, will affect the original array
        onedim_array[0] = -999
        npt.assert_array_equal([[-999, 1],
                                [2, 3],
                                [4, 5]],a)

    def test_diff_reshape_resize(self):
        """
        the difference between reshape and resize is:
        reshape, return a view with new shape
        resize, change the original array directly
        """
        oriarray = np.arange(6).reshape(2,3)

        view = oriarray.reshape(3,2)
        self.assertEqual((3,2),view.shape)
        self.assertEqual((2,3),oriarray.shape)# shape of the original array isn't changed

        # resize doesn't return anything
        # it just change the original array
        oriarray.resize((6,))
        self.assertEqual(1,oriarray.ndim)
        npt.assert_array_equal([0,1,2,3,4,5],oriarray)

        # view keep unchanged
        npt.assert_array_equal([[0, 1],
                                [2, 3],
                                [4, 5]],view)



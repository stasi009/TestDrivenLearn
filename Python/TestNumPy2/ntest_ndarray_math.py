
import unittest
import numpy as np
import numpy.testing as npt

class NTest_NdArray_Math(unittest.TestCase):

    def test_sum(self):
        b = np.arange(12).reshape(3,4)
        npt.assert_array_equal([[ 0,  1,  2,  3],
                                [ 4,  5,  6,  7],
                                [ 8,  9, 10, 11]],b)

        # when no axis is specified
        # the operator treat the array as it were a list of numbers, regardless of its shape
        self.assertEqual(66,b.sum())

        # specify the axis
        # axis = 0, calculate by column
        # axis = 1, calculate by row
        sumByColumns = b.sum(axis=0)
        npt.assert_array_equal([12,15,18,21],sumByColumns)

        sumByRows = b.sum(axis=1)
        npt.assert_array_equal([6,22,38],sumByRows)




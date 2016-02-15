
import unittest
import numpy as np
import numpy.testing as npt

class MatrixTest(unittest.TestCase):

    def test_slice_diff(self):
        arr = np.arange(1,10).reshape(3,3)

        # column is return as 1d array
        npt.assert_equal([1,4,7], arr[:,0])

        # column is return as 3*1 array
        m = np.matrix(arr)
        column = m[:,0]
        self.assertIsInstance(column,np.matrix)
        npt.assert_equal([[1],        
                          [4],        
                          [7]],column)

    def test_multiple(self):
        arr = np.arange(1,10).reshape(3,3)
        m = np.matrix(arr)

        x = np.matrix(np.ones((3,1)))

        y = m * x
        self.assertIsInstance(y,np.matrix)
        npt.assert_allclose([[6],
                             [15],
                             [24]],y)


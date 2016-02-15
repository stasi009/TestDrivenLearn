
import numpy as np
import numpy.testing as npt
import unittest

class BoolIndexTest(unittest.TestCase):
    
    def setUp(self):
        self._matrix = np.asarray([[ 0,  0,  0,  0,  0],
                                   [ 0,  1,  2,  3,  4],
                                   [ 0,  2,  4,  6,  8],
                                   [ 0,  3,  6,  9, 12],
                                   [ 0,  4,  8, 12, 16]])
        
    def test_sample1(self):
        boolindices = self._matrix >= 6
        self.assertEqual((5,5),boolindices.shape)
        npt.assert_array_equal([[False,False,False,False,False],
                                [False,False,False,False,False],
                                [False,False,False,True,True,],
                                [False,False,True,True,True],
                                [False,False,True,True,True]],boolindices)
        
        # when retrieve elements from the original matrix
        # retrieve by rows, NOT by columns (AKA, in row-wise fashion)
        selected = self._matrix[boolindices]
        self.assertEqual(1,selected.ndim)
        npt.assert_array_equal([6,8,6,9,12,8,12,16],selected)
        
    def test_sample2(self):
        rowflags = np.arange(self._matrix.shape[0])%2 == 0
        npt.assert_array_equal([True,False,True,False,True],rowflags)
        
        expected = [[ 0,  0,  0,  0,  0],
                    [ 0,  2,  4,  6,  8],
                    [ 0,  4,  8, 12, 16]]
        npt.assert_array_equal(expected,self._matrix[rowflags])
        
    def test_where_singledim(self):
        a = np.asarray([4,2,7,5,3,1])
        (indices,) = np.where(a > 3) # return a tuple
        npt.assert_array_equal([0,2,3],indices )
        
        npt.assert_array_equal([4,7,5],a[indices])
        npt.assert_array_equal([4,7,5],a[(indices,)])# same thing as above
        
    def test_where_multidim(self):
        indices = np.where(self._matrix >= 6)
        (rows,cols) = indices
        
        npt.assert_array_equal([2,2,3,3,3,4,4,4], rows)
        npt.assert_array_equal([3,4,2,3,4,2,3,4], cols)
        npt.assert_array_equal([6,8,6,9,12,8,12,16],self._matrix[indices])
        
    def test_compound(self):
        a = np.arange(10)
        filtered = (a > 4) & (a < 8) # () is necessary
        
        subarray = a[filtered]
        a[filtered] = -1*subarray
        
        npt.assert_array_equal([0,1,2,3,4,-5,-6,-7,8,9],a)
        
        
        
        
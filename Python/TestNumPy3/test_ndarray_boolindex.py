
import numpy as np
import unittest
from util4test import *

class BoolIndexTest(unittest.TestCase):
    
    def setUp(self):
        self._matrix = np.array([[ 0,  0,  0,  0,  0],
                                 [ 0,  1,  2,  3,  4],
                                 [ 0,  2,  4,  6,  8],
                                 [ 0,  3,  6,  9, 12],
                                 [ 0,  4,  8, 12, 16]])
        
    def test_sample1(self):
        boolindices = self._matrix >= 6
        self.assertEqual((5,5),boolindices.shape)
        assert_ndarray_equal(self, [[False,False,False,False,False],
                                    [False,False,False,False,False],
                                    [False,False,False,True,True,],
                                    [False,False,True,True,True],
                                    [False,False,True,True,True]],boolindices)
        
        # when retrieve elements from the original matrix
        # retrieve by rows, NOT by columns (AKA, in row-wise fashion)
        assert_ndarray_equal( self, [6,8,6,9,12,8,12,16],self._matrix[boolindices])
        
    def test_sample2(self):
        rowflags = np.arange(self._matrix.shape[0])%2 == 0
        assert_ndarray_equal(self,[True,False,True,False,True],rowflags)
        
        expected = [[ 0,  0,  0,  0,  0],
                    [ 0,  2,  4,  6,  8],
                    [ 0,  4,  8, 12, 16]]
        assert_ndarray_equal(self,expected,self._matrix[rowflags])
        
    def test_where_singledim(self):
        a = np.array([4,2,7,5,3,1])
        (indices,) = np.where(a > 3) # return a tuple
        assert_ndarray_equal( self,[0,2,3],indices )
        
        assert_ndarray_equal( self,[4,7,5],a[indices])
        assert_ndarray_equal( self,[4,7,5],a[(indices,)])
        
    def test_where_multidim(self):
        indices = np.where(self._matrix >= 6)
        (rows,cols) = indices
        
        assert_ndarray_equal( self, [2,2,3,3,3,4,4,4], rows)
        assert_ndarray_equal( self, [3,4,2,3,4,2,3,4], cols)
        assert_ndarray_equal( self, [6,8,6,9,12,8,12,16],self._matrix[indices])
        
    def test_compound(self):
        a = np.arange(10)
        filtered = (a > 4) & (a < 8) # () is necessary
        
        subarray = a[filtered]
        a[filtered] = -1*subarray
        
        assert_ndarray_equal( self,[0,1,2,3,4,-5,-6,-7,8,9],a)
        
        
        
        

import numpy as np
import unittest
from util4test import *

class NdArrayUpdateTest(unittest.TestCase):
    
    def test_by_indices(self):
        a = np.arange(1,5)
        assert_ndarray_equal( self,[1,2,3,4],a)
        
        assert_ndarray_equal(self, [2,4],a[[1,3]])
        
        a[[1,3]] = [-88,-99]
        assert_ndarray_equal(self, [1,-88,3,-99],a)
        
    def test_by_indices2(self):
        a = np.arange(1,5)
        assert_ndarray_equal( self,[1,2,3,4],a)
        
        b = [3,1]
        assert_ndarray_equal(self, [4,2],a[b])
        
        a[b] = [-88,-99]
        assert_ndarray_equal(self, [1,-99,3,-88],a)    
        
    def test_by_boolindex(self):
        a = np.arange(1,7)
        assert_ndarray_equal( self,[1,2,3,4,5,6],a)
        
        indices = (a % 2 == 0)
        a[indices] = -1*a[indices]
        assert_ndarray_equal( self,[1,-2,3,-4,5,-6],a)
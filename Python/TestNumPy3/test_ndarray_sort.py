
import numpy as np
import unittest
from util4test import *

class NdArraySortTest(unittest.TestCase):
    
    def test_npsort(self):
        original = np.array([3,2,4,1])
        sortedcopy = np.sort(original)
        
        # np.sort return a copy, so the original array is not modified
        assert_ndarray_equal(self,[3,2,4,1],original)
        assert_ndarray_equal(self,[1,2,3,4],sortedcopy)
        
        
        
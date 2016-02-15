
import unittest
import numpy as np
import numpy.testing as npt

class TestingTest(unittest.TestCase):
    
    def test_equal_onedim(self):
        a = [1,2,3]
        b = np.arange(1,4)
        # only check the content, not care about the type
        # below tests: 'a' is python's list, while 'b' is numpy's ndarray
        npt.assert_array_equal(a,b)
        
        c = [1]
        with self.assertRaises(AssertionError): 
            npt.assert_array_equal(a,c)
            
    def test_equal_multidim(self):
        a = np.asarray([[1,2,3],[4,5,6]]).T
        b = np.c_[[1,2,3],[4,5,6]]
        npt.assert_array_equal(a,b)
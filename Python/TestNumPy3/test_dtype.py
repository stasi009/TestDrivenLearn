
import numpy as np
import unittest
from util4test import *

class DTypeTest(unittest.TestCase):
    
    def test_array_type(self):
        a = np.array([1,2])
        self.assertEqual(np.int32,a.dtype)
        self.assertEqual(int,a.dtype)
        self.assertNotEqual(np.int64,a.dtype)
        
    def test_specify_dtype_whencreate(self):
        b = np.ones((2,2))
        self.assertEqual(np.float,b.dtype)
        
        c = np.ones((2,2),dtype=np.int)
        self.assertEqual(np.int,c.dtype)       
        
    def test_astype(self):
        a = np.array([1.1,2.9,3.5])
        self.assertEqual(np.float,a.dtype)
        
        # just truncate, not round
        b = a.astype(int)
        assert_ndarray_equal( self,[1,2,3],b)
        
        # array returned is an isolated copy, not the view of the original
        b[1] = -99
        self.assertAlmostEqual( 2.9,a[1])
        
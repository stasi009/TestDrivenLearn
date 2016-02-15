
import numpy as np
import unittest
import io
from util4test import *

class TxtIoTest(unittest.TestCase):
    
    def test_loadtxt_simple(self):
        f = io.StringIO("0 1\n2 3")

        a = np.loadtxt(f)
        self.assertEqual(np.float,a.dtype)
        
        assert_ndarray_almost_equal( self,[[0.0,1.0],
                                           [2.0,3.0]],a)
        
    def test_loadtxt_dtype(self):
        f = io.StringIO("Male 21 72\nFemale 35 58")
        a = np.loadtxt(f, dtype={
            'names': ('gender', 'age', 'weight'),
            'formats': ('S8', 'i4', 'f4')})   
        assert_ndarray_equal( self, [b"Male",b"Female"],a["gender"])
        assert_ndarray_equal( self, [21,35],a["age"])
        assert_ndarray_equal( self, [72,58],a["weight"])
        
    def test_loadtxt_sample1(self):
        f = io.StringIO("1,0,2\n3,0,4")
        a = np.loadtxt(f, delimiter=',', usecols=(0, 2))
        assert_ndarray_almost_equal( self,[[1.0,2.0],[3.0,4.0]],a)
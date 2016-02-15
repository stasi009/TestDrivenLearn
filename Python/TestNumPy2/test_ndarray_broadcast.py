
import numpy as np
import numpy.testing as npt
import unittest
from util4test import *

class NdBroadcastTest(unittest.TestCase):
    
    def test_simple_sameshape(self):
        a = np.arange(1,4)
        npt.assert_array_equal( [2,4,6],a+a)
        npt.assert_array_equal( [1,4,9],a*a)
        
    def test_broadcast(self):
        a = np.arange(1,4).reshape((3,1))
        b = np.arange(1,3)
        
        # a is repeated in horizontal direction, enlarge to 3*2 matrix
        # b is repeated in vertical direction, enlarge to 3*2 matrix
        # and then add together
        c = a + b
        npt.assert_array_equal ([[2, 3],
                                 [3, 4],
                                 [4, 5]],c)
        
    def test_sample_normalize(self):
        a = [[1,4],[5,2]]
        a = np.asarray(a)
        
        colmean = a.mean(0)
        assert_ndarray_equal(self,[3,3],colmean)
        assert_ndarray_equal(self,[[-2,1],[2,-1]],a - colmean)
        
        colmax = a.max(0) * 1.0
        assert_ndarray_almost_equal(self,[[0.2,1],[1,0.5]],a/colmax)
        
    def test_multiply_diagmatrix(self):
        # broadcasting feature can be used to multiply a matrix with a diagonal matrix
        # in a much convenient way
        m = np.c_[[1,2,3],[4,5,6]]
        diagelements = [2,3]
        
        expected = np.c_[[2,4,6],[12,15,18]]
        
        # convenient way to multiply
        result1 = m * diagelements
        
        # multiply in a matrix fashion
        result2 = np.dot(m,np.diag(diagelements))
        
        # results are equal
        npt.assert_array_equal(result1,result2)
        npt.assert_array_equal(expected,result1)
        self.assertTrue(np.array_equal(result1,result2))
        
        
    def test_cannot_broadcast(self):
        """
        if the longer array is not multiple times of the shorter array
        in R, it will just give a warning
        however, in numpy, it will throw an exception
        """
        a = np.array([1,2,3,4])
        b = np.array([5,6])
        with self.assertRaises(ValueError):
            a+b
        
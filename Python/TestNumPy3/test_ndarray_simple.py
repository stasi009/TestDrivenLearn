
import numpy as np
import numpy.testing as npt
import unittest
import math
from util4test import *

class NdArraySimpleTest(unittest.TestCase):
    
    def test_build_multidim_array(self):
        actual = np.array([   [r-c for c in range(4)]         for r in range(5) ]  )
        expected = np.array([[ 0, -1, -2, -3],
                          [ 1,  0, -1, -2],
                          [ 2,  1,  0, -1],
                          [ 3,  2,  1,  0],
                          [ 4,  3,  2,  1]])   
        self.assertTrue(np.array_equal(expected,actual))
        self.assertEqual(1,actual[2,1])
    
    def test_shape_dim1(self):
        a = np.array([1,2,3,4,5])
        self.assertEqual(1,a.ndim)
        self.assertEqual((5,),a.shape)
        
        b = np.array([[1,2,3],[4,5,6]])
        self.assertEqual(2,b.ndim)
        self.assertEqual((2,3),b.shape)
        
    def test_shape_dim2(self):
        oneArray = np.array([1,2])
        self.assertEqual(1,oneArray.ndim)
        self.assertEqual((2,),oneArray.shape)
        
        matrixOneRow = np.array( [ [1,2] ] )
        self.assertEqual(2,matrixOneRow.ndim)        
        self.assertEqual((1,2),matrixOneRow.shape)
        
    def test_index(self):
        a = np.array([[1,2,3],[4,5,6]])
        self.assertEqual(4,a[1,0])
        self.assertEqual(2,a[0,1])
    
    def test_sum(self):
        a = np.array([5,4,15])
        self.assertEqual(24,np.sum(a))
        self.assertEqual(24,a.sum())
        
    def test_flatten(self):
        """flatten performs in row fashion"""
        a = np.array([[1,2],[3,4],[5,6]])
        self.assertEqual((3,2),a.shape)
        assert_ndarray_equal(self,[1,2,3,4,5,6],a.flatten())
        
    def test_ufuncs(self):
        """
        'numpy' provide a set of functions whose name and function are similar to their counterparts in the 'math' module
        however, the difference is that, for example,
        math.sin operates on a single number, while
        numpy.sin operates on an array. 
        those numpy functions called 'universal functions', they performs math operations element-wise
        operate each element one by one
        """
        x = np.linspace(0, 2*math.pi, 10)
        y = np.sin(x)
        npt.assert_allclose( [  0.00000000e+00,   6.42787610e-01,   9.84807753e-01,
                                8.66025404e-01,   3.42020143e-01,  -3.42020143e-01,
                                -8.66025404e-01,  -9.84807753e-01,  -6.42787610e-01,
                                -2.44929360e-16],y )   
        
    def test_unique(self):
        a = np.array([1,9,1,1,2,4,2,2,9,9,9,3,3,3])
        expected = [1,2,3,4,9] # not only unique but also sorted
        npt.assert_equal(expected,np.unique(a))
        
    def test_copy(self):
        original = np.array([[1,2,3],
                             [4,5,6],
                             [7,8,9]])    
        ref = original
        cpy = np.copy(original)
        
        original[1,1] = -1000
        self.assertEqual(-1000,ref[1,1])
        self.assertEqual(5,cpy[1,1])
        
        
    def test_norm(self):
        """just the length of the vector, sqrt(x*x + y*y + z*z)"""
        a = np.array([1,2,3,4])
        self.assertAlmostEqual(math.sqrt(30),np.linalg.norm(a))
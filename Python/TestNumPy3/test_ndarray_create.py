
import numpy as np
import unittest
from util4test import *

class NdArrayCreateTest(unittest.TestCase):

    def test_specify_dtype(self):
        floatmatrix = np.ones((2,2))
        self.assertEqual(np.float,floatmatrix.dtype)
        
        intmatrix = np.ones((2,2),dtype=np.int)
        self.assertEqual(np.int,intmatrix.dtype)
        
    def test_empty(self):
        # unlike "zeros", the returned array isn't initialized
        # it is just filled with random values
        a = np.empty((2,2))
        self.assertEqual((2,2),a.shape)
        self.assertEqual(np.float,a.dtype)
        
        a[:] = [[1,2],[3,4]]
        assert_ndarray_almost_equal( self,[[1,2],[3,4]],a)
    
    def test_from_listcomprehension_1(self):
        actual = np.array([x*x for x in range(1,5)])
        expected = np.array([1,4,9,16])
        self.assertTrue(np.array_equal(expected,actual))
        
        # == not return a single boolean, but return an array of boolean
        # == also function in a "broadcast way"
        compare = (actual == expected)
        self.assertEqual((4,),compare.shape)
        self.assertTrue(compare.all)
        
    def test_from_listcomprehension_2(self):
        actual = np.array([   [r-c for c in range(4)]         for r in range(5) ]  )
        expected = np.array([[ 0, -1, -2, -3],
                             [ 1,  0, -1, -2],
                             [ 2,  1,  0, -1],
                             [ 3,  2,  1,  0],
                             [ 4,  3,  2,  1]])   
        self.assertTrue(np.array_equal(expected,actual))
        self.assertEqual(1,actual[2,1]) 
        
    def test_zeros(self):
        actual = np.zeros((3,))# need a tuple, so "," is necessary
        self.assertEqual(np.float,actual.dtype)
        self.assertTrue( compare_equal([0.0,0.0,0.0],actual))
        
    def test_zeros_like(self):
        a = np.random.rand(2,3)
        b = np.zeros_like(a)
        self.assertEqual((2,3),b.shape)
        self.assertTrue( compare_equal([[0,0,0],[0,0,0]],b) )
        
    def test_ones(self):
        actual = np.ones((3,3))
        self.assertEqual(np.float,actual.dtype)
        self.assertTrue(compare_equal([[ 1.,  1.,  1.],
                                       [ 1.,  1.,  1.],
                                       [ 1.,  1.,  1.]],actual))
        
    def test_ones_like(self):
        a = np.random.rand(2,3)
        b = np.ones_like(a)
        self.assertEqual((2,3),b.shape)
        self.assertTrue( compare_equal([[1,1,1],[1,1,1]],b) )        
        
    def test_eye(self):
        self.assertTrue( compare_equal([[ 1.0,  0.0,  0.0],
                                        [ 0.0,  1.0,  0.0],
                                        [ 0.0,  0.0,  1.0]],np.eye(3)))
        
    def test_arange(self):
        """remember that the last point is always excluded in ranges but included in the result of linspace"""
        startpnt = 1
        endpnt = 5
        interval = 2
        self.assertTrue( compare_equal([0,1,2,3,4],np.arange(endpnt)) )
        self.assertTrue( compare_equal([1,2,3,4],np.arange(startpnt,endpnt)) )
        self.assertTrue( compare_equal([1,3],np.arange(startpnt,endpnt,interval)) )
        
    def test_arange_types(self):
        intarray = np.arange(1,5,2)
        self.assertEqual(int,intarray.dtype)
        assert_ndarray_equal( self,[1,3],intarray)
        
        floatarray = np.arange(1,2,0.3)
        self.assertEqual(float,floatarray.dtype)
        assert_ndarray_almost_equal(self,[1.0,1.3,1.6,1.9],floatarray)
        
    def test_linspace(self):
        """remember that the last point is always excluded in ranges but included in the result of linspace"""
        startpnt = 1
        endpnt = 2
        numsplits = 3
        self.assertTrue( compare_almost_equal([ 1. ,  1.5,  2. ],np.linspace(startpnt,endpnt,numsplits)) )
        
        numsplits = 4
        self.assertTrue( compare_almost_equal([ 1.0 ,  1.33333333,  1.66666667,  2.0 ],np.linspace(startpnt,endpnt,numsplits)) )
        
        
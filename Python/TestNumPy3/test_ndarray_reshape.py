
import numpy as np
import unittest
from util4test import *

class NdArrayReshapeTest(unittest.TestCase):
    
    def test_stack(self):
        a = np.array([[1,2],[3,4]])
        assert_ndarray_equal( self,[[1,2],[3,4],[1,2],[3,4],[1,2],[3,4]],np.vstack([a,a,a]) )
        assert_ndarray_equal( self,[[1,2,1,2],[3,4,3,4]],np.hstack([a,a]) )
        
    def test_reshape(self):
        """reshape, elements are taken and filled in row-wise order"""
        original = np.array([[1, 2, 3, 4],
                             [5, 6, 7, 8]])
        
        reshaped = original.reshape(4,2)
        # in row-wise order
        assert_ndarray_equal( self,[[1,2],
                                    [3,4],
                                    [5,6],
                                    [7,8]],reshaped )
        self.assertEqual((2,4),original.shape)
        
    def test_reshape_view(self):
        """
        array returned by reshape is just view of the original
        so modify on the returned array will directly impact the original one
        and vice versa
        """
        original = np.array([[1, 2, 3, 4],
                             [5, 6, 7, 8]])
                
        reshaped = original.reshape(4,2)
        # in row-wise order
        assert_ndarray_equal( self,[[1,2],
                                    [3,4],
                                    [5,6],
                                    [7,8]],reshaped )
        
        ################## modify on the returned view, impact the original directly
        reshaped[3,0] = -999
        self.assertEqual(-999,original[1,2])
        
        ################## modify on the original, impact the returned view directly
        original[0,2] = -888
        self.assertEqual(-888,reshaped[1,0])
        
        ##################
        assert_ndarray_equal( self,[[1, 2, -888, 4],
                                    [5, 6, -999, 8]],original)
        assert_ndarray_equal( self,[[1,2],
                                    [-888,4],
                                    [5,6],
                                    [-999,8]],reshaped)
        
        
    def test_reshape_standalone_func(self):
        """reshape, elements are taken and filled in row-wise order"""
        original = np.array([[1, 2, 3, 4],
                             [5, 6, 7, 8]])
        
        reshaped = np.reshape(original,(4,2))
        # in row-wise order
        assert_ndarray_equal( self,[[1,2],
                                    [3,4],
                                    [5,6],
                                    [7,8]],reshaped )
        self.assertEqual((2,4),original.shape)        
        
    def test_transpose_work(self):
        original = np.array([[0, 1, 2, 3],
                             [0, 1, 2, 3],
                             [0, 1, 2, 3]])
        transposed = np.transpose(original)
        
        assert_ndarray_equal (self,[[0, 0, 0],
                                    [1, 1, 1],
                                    [2, 2, 2],
                                    [3, 3, 3]],transposed)        
        
        # return a new one, the original matrix is not changed
        self.assertEqual((3,4),original.shape)
        self.assertEqual((4,3),transposed.shape)
        
    def test_transpose_notwork(self):
        """transpose only work for array whose dimension larger than 1"""
        a = np.array([1,2])
        self.assertEqual(1,a.ndim)
        
        b = np.transpose(a)
        self.assertTrue(np.array_equal(b,a)) # not transposed
        
        ###################### make it multi-dimensional
        c = np.array([[3,4]])
        self.assertEqual((1,2),c.shape)
        
        d = np.transpose(c)
        assert_ndarray_equal( self,[[3],[4]],d )
        
        
    def test_flatten(self):
        """flatten performs in row fashion"""
        a = np.array([[1,2],[3,4],[5,6]])
        self.assertEqual((3,2),a.shape)
        assert_ndarray_equal(self,[1,2,3,4,5,6],a.flatten())    
        
    def test_squeeze(self):
        a = np.array([[0],
                      [1],
                      [2],
                      [3]])
        self.assertEqual((4,1),a.shape)
        
        squeezed = np.squeeze(a)
        self.assertEqual((4,),squeezed.shape)
        assert_ndarray_equal( self,[0,1,2,3],squeezed)
        
        ##################### for scalar number
        oneelem = np.array([[6]])
        self.assertEqual((1,1),oneelem.shape)
        
        scalar = np.squeeze(oneelem)
        self.assertEqual(6,scalar)
        self.assertEqual(6,int(scalar))
        self.assertTrue(not isinstance(scalar,int))
        
        with self.assertRaises(IndexError):
            # zero-dim, cannot be indexed
            self.assertEqual(6,scalar[0])
        
        self.assertEqual(0,scalar.ndim)
        self.assertEqual((),scalar.shape)
        
        
        ##################### it only works against singular dimension matrix
        notwork = np.array([[1,2],[3,4],[5,6]])
        self.assertEqual((3,2),notwork.shape)
        assert_ndarray_equal(self,[[1,2],[3,4],[5,6]],np.squeeze(notwork))        
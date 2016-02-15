
import numpy as np
import numpy.testing as npt
import unittest

class NdArrayLinAlgTest(unittest.TestCase):
    
    def test_dot_vector(self):
        """here, 'vector' means 1-dimensional array"""
        v = np.asarray([5,6])
        x = np.dot(v,v)
        self.assertEqual(61,x)
        
        self.assertEqual((),x.shape)
        self.assertEqual(0,x.ndim)
        self.assertEqual(61,int(x))
        
    def test_matrix_math1(self):
        A = np.asarray([[1,2],[3,4]])
        self.assertEqual((2,2),A.shape)
        
        v = np.asarray([5,6])
        self.assertEqual((2,),v.shape)
        
        # first, v (originally one dim) is extend to 2*1
        # multiply, then result convert back to a vector (not a matrix)
        npt.assert_array_equal( [17, 39],np.dot(A,v))
        
        # first, v (originally one dim) is extend to 1*2
        # multiply, then result convert back to a vector (not a matrix)        
        npt.assert_array_equal( [23, 34],np.dot(v,A))
        
    def test_matrix_math2(self):
        v = np.array([8,9]).reshape(2,1)
        npt.assert_array_equal( [[64,72],[72,81]],np.dot(v,np.transpose(v)) )
        
        oneelem = np.dot(np.transpose(v),v)
        self.assertEqual(2,oneelem.ndim)
        self.assertEqual((1,1),oneelem.shape)
        npt.assert_array_equal([[145]],oneelem )
        self.assertEqual(145,oneelem)
        self.assertEqual(145,int(oneelem))
        
        
        
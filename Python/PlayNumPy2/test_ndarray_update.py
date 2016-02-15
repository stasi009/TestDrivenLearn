
import unittest
import math
import numpy as np
import numpy.testing as npt

class NdArrayUpdateTest(unittest.TestCase):

    def test_inplace_modify(self):

        a= np.asarray([(1,2),(3,4)])
        b = np.asarray([(5,6),(7,8)])

        a+=b
        npt.assert_equal([[ 6,  8],       
                          [10, 12]],a)

        # cannot cast float64 array to int array
        c = 1.9 * np.ones((2,2))
        self.assertRaises(TypeError,a+c)

    def test_upcast(self):
        a= np.asarray([ (1.1,2.2),(3.3,4.4) ])
        b = np.ones((2,2),dtype=int)

        # when doing math between float array and int array, int array is upcast to float array
        npt.assert_allclose([(2.1,3.2),                          
                             (4.3,5.4)],a+b)

    def test_update_by_slice(self):
        a = np.arange(6).reshape(2,3)
        npt.assert_equal([[0, 1, 2],        
                          [3, 4, 5]],a)

        a[:,1] *= -1
        npt.assert_equal([[0, -1, 2],        
                          [3, -4, 5]],a)

        a[-1,:] = 0
        npt.assert_equal([[0, -1, 2],        
                          [0, 0, 0]],a)

    def test_update_by_slice_broadcast(self):
        a = np.arange(6)
        npt.assert_equal([1,2,3],a[1:4])

        a[1:4] = -99 # can be broadcast to [-99,-99,-99]
        npt.assert_equal([0,-99,-99,-99,4,5],a)

        with self.assertRaises(ValueError):
            a[1:4] = [-99,-100] # cannot be broadcast to three-element array

    def test_update_by_bool_index(self):
        a = np.arange(6)

        b = a%2==0
        npt.assert_array_equal([True,False,True,False,True,False],b)

        # boolean slice will return a copy
        sliced_copy = a[b]
        npt.assert_equal([0,2,4],sliced_copy)

        sliced_copy *= -1
        npt.assert_equal([0,-2,-4],sliced_copy)
        npt.assert_equal([0,1,2,3,4,5],a) # since bool indexing return a copy, so the original isn't modified

        # !!! but bool indexing can directly be updated
        a[b] *= -1
        npt.assert_equal([0,1,-2,3,-4,5],a) 


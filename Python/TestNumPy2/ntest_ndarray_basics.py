
import unittest
import numpy as np
import numpy.testing as npt

class NTest_NdArray_Basics(unittest.TestCase):

    def test_iterate(self):
        a = np.arange(6).reshape((2,3))

        # iterate directly, return rows
        npt.assert_array_equal([[0,1,2],
                                [3,4,5]],[row for row in a])

        # use "flat" to iterate each elements
        element_iterator = a.flat
        self.assertEqual(0,next(element_iterator))
        self.assertEqual(1,next(element_iterator))
        self.assertEqual([2,3,4,5],list(element_iterator))

        # !!! weird, it seems, assert_array_equal cannot be used with "iterator" type
        # npt.assert_array_equal([2,3,4,5],element_iterator)


    def test_shapes(self):
        a = np.arange(15).reshape(3, 5)
        self.assertEqual(2,a.ndim)
        self.assertEqual((3,5),a.shape)
        self.assertEqual(15,a.size)

    def test_onedim_array(self):
        onedim_array = np.asarray([8,9])
        self.assertEqual(1,onedim_array.ndim)
        self.assertEqual((2,),onedim_array.shape)

        twodim_array = np.asarray( [[8,9]] )
        self.assertEqual(2,twodim_array.ndim)
        self.assertEqual((1,2),twodim_array.shape)

    def test_dtype(self):
        a = np.arange(15).reshape(3, 5)
        self.assertEqual(int,a.dtype)
        self.assertEqual(np.int,a.dtype)
        self.assertEqual("int32",a.dtype.name)

        # ndarray.itemsize. the size in bytes of each element of the array. 
        self.assertEqual(4,a.itemsize)


    def test_complex_array1(self):
        c = np.asarray( [ [1,2], [3,4] ], dtype=complex )
        expected = np.asarray([  [ 1.+0.j,  2.+0.j],                         
                                 [ 3.+0.j,  4.+0.j]  ])
        npt.assert_array_equal(expected,c)


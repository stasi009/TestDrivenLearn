
import unittest
import numpy as np
import numpy.testing as npt

class NumPyDtypeTest(unittest.TestCase):

    def test_demo1(self):
        a = np.asarray([1,2])
        self.assertEqual(np.int,a.dtype)
        self.assertEqual(int,a.dtype)

        b = np.asarray([1.1,2.2])
        self.assertEqual(np.float,b.dtype)
        self.assertEqual(float,b.dtype)

    def test_specify_type_whencreate(self):
        b = np.ones((2,2))
        self.assertEqual(np.float,b.dtype)
        
        c = np.ones((2,2),dtype=np.int)
        self.assertEqual(np.int,c.dtype)     
        
        d = np.arange(3,dtype=np.float)
        self.assertEqual(np.float,d.dtype)
        self.assertEqual("float64",d.dtype.name)
        npt.assert_allclose([0.0,1.0,2.0],d) 

    def test_astype(self):
        a = np.array([1.1,2.9,3.5])
        self.assertEqual(np.float,a.dtype)
        
        # just truncate, not round
        b = a.astype(int)
        npt.assert_array_equal([1,2,3],b)
        
        # array returned is an isolated copy, not the view of the original
        b[1] = -99
        self.assertAlmostEqual(2.9,a[1])

    def test_itemsize(self):
        # -------- int array, 4 bytes
        int_array = np.arange(3)
        self.assertEqual(np.int,int_array.dtype)
        self.assertEqual(4,int_array.itemsize)

        # -------- float array (shorthand for float64), 8 bytes
        float_array = np.zeros((2,3))
        self.assertEqual(np.float,float_array.dtype)
        self.assertEqual(np.float64,float_array.dtype)
        self.assertEqual(8,float_array.itemsize)



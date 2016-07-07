
import unittest
import numpy as np
import numpy.testing as npt

class NdArrayCreateTest(unittest.TestCase):

    def test_create_from_iterator(self):
        # !!!  here, we cannot set iterator=xrange(5)
        # !!!  xrange(5) return not generator, it is "xrange" type, similar to
        # generator, but not exactly is
        # !!!  asarray(xrange(5)) will return array([0, 1, 2, 3, 4])
        # !!!  asarray(generator) will return array([generator])
        iterator = (x for x in xrange(5))
        iterator_array = np.asarray(iterator)
        self.assertEqual(1,iterator_array.size)# build an array with one iterator as its element, not what we want

        array1 = np.fromiter(iterator,int)
        npt.assert_array_equal([0,1,2,3,4],array1)

    def test_fromfunction(self):
        a = np.fromfunction(lambda i, j: i + j, (3, 3), dtype=int)
        npt.assert_array_equal([[0, 1, 2],                               
                                [1, 2, 3],                               
                                [2, 3, 4]],a)

    def test_asarray_copy_ref(self):
        # on non-ndarray, return an isolated copy
        a = [1,2]
        b = np.asarray(a)

        a[0] = -99
        # a copy is made, so change on the original, won't affect the other
        self.assertEqual(1,b[0])

        # on ndarray, no copy is made, just return the reference to input ndarray
        # that is the difference between "np.array" and "np.asarray"
        acopy = np.array(b)
        self.assertIsNot(acopy,b)
        npt.assert_equal(acopy,b)

        aref = np.asarray(b)
        self.assertIs(aref,b)
    
    def test_asarray(self):
        a = [1,2]
        b = np.asarray(a)
        npt.assert_equal(a,b)
        self.assertEqual(np.int,b.dtype)

        # if the input is already ndarray, then no copy is made
        self.assertIs(np.asarray(b),b)

        # can specify the type when coverting
        c = np.asarray(a,dtype=np.float)
        self.assertEqual(np.float,c.dtype)
        npt.assert_equal(c,[1.0,2.0])

        # ------------------ upcast 
        b = [1,6.9] # python list can have mixed type
        bb = np.asarray(b) # since ndarray can have only one type, so different types should be upcasted to a general type
        self.assertEqual(np.float,bb.dtype)

    def test_asarray_multidim(self):
        # convert a list of tuple
        b = np.asarray([(1,2,3),
                        (4,5,6)]) # each element will become a row
        self.assertEqual(2,b.ndim)
        self.assertEqual((2,3),b.shape)

        npt.assert_equal([1,2,3],b[0])# 0-th row
        npt.assert_equal([2,5],b[:,1])# 1-th column

        # convert a list of list
        c = np.asarray([[1],
                        [3]])
        self.assertEqual((2,1),c.shape)

    def test_specify_dtype(self):
        # ----------------------------------- #
        floatmatrix = np.ones((2,2))
        self.assertEqual(np.float,floatmatrix.dtype)
        npt.assert_allclose([[1.0,1.0],[1.0,1.0]],floatmatrix)
        
        # ----------------------------------- #
        intmatrix = np.ones((2,2),dtype=np.int)
        self.assertEqual(np.int,intmatrix.dtype)

        # ----------------------------------- #
        float_array = np.arange(3,dtype=np.float)
        self.assertEqual(np.float,float_array.dtype)
        self.assertEqual("float64",float_array.dtype.name)
        self.assertEqual(8,float_array.itemsize)
        npt.assert_allclose([0.0,1.0,2.0],float_array)
        
    def test_init_with_same_value(self):
        """
        use full or full_like to create and initialize an array filled with same value
        """
        # by default, it will initialize as float array
        npt.assert_allclose([[ 9.,  9.,  9.],
                             [ 9.,  9.,  9.]], np.full((2,3),9))

        # specify the data type
        npt.assert_equal([[ 9,  9,  9],                        
                          [ 9,  9,  9]], np.full((2,3),9,dtype=np.int))

        # full_like
        x = np.ones((3,4))
        npt.assert_equal([[8, 8, 8, 8],
                          [8, 8, 8, 8],
                          [8, 8, 8, 8]],  np.full_like(x,8,dtype=np.int))

    def test_empty(self):
        # unlike "zeros", the returned array isn't initialized
        # it is just filled with random values
        a = np.empty((2,2))
        self.assertEqual((2,2),a.shape)
        self.assertEqual(np.float,a.dtype)
        self.assertEqual("float64",a.dtype.name)
        self.assertEqual(8,a.itemsize)
        
        a[:] = [[1,2],[3,4]]
        npt.assert_array_equal([[1,2],[3,4]],a)

    def test_arange(self):
        """remember that the last point is always excluded in ranges but included in the result of linspace"""
        npt.assert_equal([0,1,2,3,4],np.arange(5))
        npt.assert_equal([1,2,3,4],np.arange(1,5))

        # use stripe
        npt.assert_equal([1,3],np.arange(1,5,2))
        npt.assert_allclose([1. ,  1.5,  2. ,  2.5,  3. ,  3.5,  4. ,  4.5],np.arange(1,5,0.5))

        # use negative stripe
        npt.assert_equal([5,4,3,2],np.arange(5,1,-1))
        npt.assert_allclose([5. ,  4.5,  4. ,  3.5,  3. ,  2.5,  2. ,  1.5],np.arange(5,1,-0.5))

    def test_linspace(self):
        """remember that the last point is always excluded in aranges but included in the result of linspace"""
        startpnt = 1
        endpnt = 2
        numsplits = 3
        npt.assert_array_equal([1. ,  1.5,  2.],np.linspace(startpnt,endpnt,numsplits)) 
        
        numsplits = 4
        npt.assert_allclose([1.0 ,  1.33333333,  1.66666667,  2.0],np.linspace(startpnt,endpnt,numsplits)) 

    def test_zeros_ones(self):
        shape = (2,3)

        # ============================= one-dimensonal array
        npt.assert_allclose([ 0.,  0.,  0.],np.zeros(3))
        npt.assert_allclose([ 1.,  1.,  1.],np.ones(3))

        # ============================= zeros
        zeros1 = np.zeros(shape)# need a tuple
        zeros2 = np.zeros_like(np.random.rand(*shape))# rand need separate parameters
        self.assertEqual(np.float,zeros1.dtype)
        self.assertEqual(np.float,zeros2.dtype)
        
        expected = np.asarray([[0.,  0.,  0.],       [0.,  0.,  0.]])
        npt.assert_allclose(expected,zeros1)
        npt.assert_allclose(expected,zeros2)

        # ============================= ones
        ones1 = np.ones(shape)# need a tuple
        ones2 = np.ones_like(np.random.rand(*shape))# rand need separate parameters
        self.assertEqual(np.float,ones1.dtype)
        self.assertEqual(np.float,ones2.dtype)

        expected = np.asarray([[1.0,  1.0,  1.0],       [1.0,  1.0,  1.0]])
        npt.assert_allclose(expected,ones1)
        npt.assert_allclose(expected,ones2)

        # ============================= specify the type
        npt.assert_array_equal([(1,1,1),
                                (1,1,1)],np.ones((2,3),dtype=int))

        # ============================= eye
        npt.assert_allclose([[1.,  0.,  0.], [0.,  1.,  0.],  [0.,  0.,  1.]],np.eye(3))
        npt.assert_allclose([[1.,  0.,  0.], [0.,  1.,  0.],  [0.,  0.,  1.]],np.identity(3))



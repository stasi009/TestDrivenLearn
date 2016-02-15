
import unittest
import numpy as np
import numpy.testing as npt

class NTest_NdArray_Create(unittest.TestCase):

    def test_create_from_iterator(self):
        iterator = (x for x in xrange(5))
        iterator_array = np.asarray(iterator)
        self.assertEqual(1,iterator_array.size)# build an array with one iterator as its element, not what we want

        array1 = np.fromiter(iterator,int)
        npt.assert_array_equal([0,1,2,3,4],array1)

    def test_fromfunction(self):
        a = np.fromfunction(lambda i, j: i + j, (3, 3), dtype=int)
        expected = np.asarray([[0, 1, 2],
                               [1, 2, 3],
                               [2, 3, 4]])
        npt.assert_array_equal(expected,a)

    def test_copy(self):
        a = np.asarray([(1,2),
                        (3,4)])

        # copy will return a isolated copy
        cpy = a.copy()

        # changes on copy will not affect original
        cpy[1,1] = -999
        npt.assert_array_equal([[   1,    2],
                                [   3, -999]],cpy)
        npt.assert_array_equal([(1,2),
                                (3,4)],a)


    def test_zeros_ones(self):
        """
        Often, the elements of an array are originally unknown, but its size is known. 
        Hence, NumPy offers several functions to create arrays with initial placeholder content. 
        These minimize the necessity of growing arrays, an expensive operation.
        """
        zeros = np.zeros((3,4))
        # by default, the type is float
        expected_zeros = np.asarray([[0.,  0.,  0.,  0.],
                                     [0.,  0.,  0.,  0.],
                                     [0.,  0.,  0.,  0.]])
        npt.assert_allclose(expected_zeros,zeros)

        # specify the type when creating the array
        ones = np.ones((2,3),dtype=int)
        expected_ones = np.asarray ( [(1,1,1),(1,1,1)] )
        npt.assert_array_equal(expected_ones,ones)

    def test_empty(self):
        """
        empty creates an array whose initial content is random and depends on the state of the memory
        """
        a = np.empty((2,3))
        self.assertEqual("float64",a.dtype.name)
        self.assertEqual(8,a.itemsize)

    def test_asarray(self):
        a1 = np.asarray([1,2,3])
        npt.assert_array_equal([1,2,3],a1)# compare not just ndarray, but accept array-like object

        # the difference between np.array and np.asarray is:
        # when the input is already a ndarray
        # np.array will make copy, while np.asarray will return the reference to the original ndarray
        acopy = np.array(a1)
        self.assertIsNot(acopy,a1)

        aref = np.asarray(a1)
        self.assertIs(aref,a1)


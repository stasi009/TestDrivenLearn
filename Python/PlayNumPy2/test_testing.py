
import unittest
import numpy as np
import numpy.testing as npt

class NumPyTestingTest(unittest.TestCase):

    def test_array_equal(self):
        a = [1,2,3]
        b = np.arange(1,4)
        # one is python list, another is numpy ndarray
        # they are still the same, based on their content
        npt.assert_array_equal(a,b)

        with self.assertRaises(AssertionError):
            npt.assert_array_equal([1],b)

    def test_equal(self):
        a = [1,2,3]
        b = np.arange(1,4)
        # one is python list, another is numpy ndarray
        # they are still the same, based on their content
        npt.assert_equal(a,b)

        with self.assertRaises(AssertionError):
            npt.assert_equal([1],b)

    def test_all_close(self):
        a = np.arange(3)
        b = a + 0.001

        npt.assert_allclose(a,b,atol=0.01)# here use the absolute tolerance

        with self.assertRaises(AssertionError):
            npt.assert_allclose(a,b,atol=0.000001)# here use the absolute tolerance

    def test_multi_dimensions(self):
        a = np.asarray([[1,2,3],[4,5,6]]).T
        b = np.c_[[1,2,3],[4,5,6]]
        npt.assert_array_equal(a,b)
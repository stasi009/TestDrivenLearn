
import unittest
import numpy as np
import numpy.testing as npt

class NdArrayCompareTest(unittest.TestCase):

    def test_equal(self):
        a = [1,2]
        b = np.asarray(a)

        self.assertTrue(a is not b)
        # one is python list, another is ndarray
        # they are still "equal" based on their same content
        self.assertTrue(np.array_equal(a,b))

    def test_equal_multidim(self):
        a = [[1,2],[3,4]]
        b = np.asarray(a)

        self.assertTrue(a is not b)
        # one is python list, another is ndarray
        # they are still "equal" based on their same content
        self.assertTrue(np.array_equal(a,b))

    def test_allclose(self):
        a = np.arange(3)
        b = a + 0.001

        self.assertTrue(np.allclose(a,b,atol=0.01))# use the absolute tolerance
        self.assertFalse(np.allclose(a,b,atol=0.00001))# use the absolute tolerance

    def test_equivalent(self):
        """
        Returns True if input arrays are shape consistent and all elements equal.
        Shape consistent means they are either the same shape, 
        or one input array can be broadcasted to create the same shape as the other one.
        """
        a = [1,2]
        b = np.asarray(a)
        self.assertTrue(np.array_equiv(a, b))
        self.assertTrue(np.array_equiv(a,[a,a]))

    def test_equal_operator(self):
        a = np.asarray([1,2,3])
        # ------------ same length, element-wise checking
        npt.assert_equal([True,True,True],a==[1,2,3])
        npt.assert_equal([True,False,True],a==[1,6,3])
        # ------------ different length, always return false
        self.assertFalse(a==[1,2])
        # ------------ single number, broadcasting, and then element-wise checking
        npt.assert_equal([True,False,False],a==1)
        npt.assert_equal([False,False,True],a==[3])


        





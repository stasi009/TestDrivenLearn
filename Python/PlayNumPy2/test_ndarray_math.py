
import unittest
import math
import numpy as np
import numpy.testing as npt

class NdArrayMathTest(unittest.TestCase):

    def test_round(self):
        a = [-1.1,2.6,3.9]
        rounded = np.rint(a)
        self.assertEqual(np.float,rounded.dtype)
        npt.assert_allclose([-1.,  3.,  4.],rounded)

    def test_dot_inner_product(self):
        """
        If `a` and `b` are both scalars or both 1-D arrays then performing inner product (without complex conjugation)
        a scalar is returned
        """
        a = np.asarray([1,2])
        b = np.asarray([3,4])
        self.assertEqual(11,np.dot(a,b))

    def test_dot_multiply_matrix(self):
        # --------------- two matrix
        a = np.asarray([[1],
                       [2]])
        b = np.asarray([[3,4]])
        npt.assert_equal([[3,4],
                          [6,8]],np.dot(a,b))

        # --------------- one matrix and one vector
        # the vector doesn't need to be a column vector, np.dot can transform
        # it into column vector automatically
        a1 = np.asarray([[1,2],[3,4]])
        b1 = np.asarray([5,6])
        npt.assert_equal([17,39],np.dot(a1,b1))

    def test_trace(self):
        a = np.asarray([[1,2],[3,4]])
        self.assertEqual(5,np.trace(a))# trace: sum of diagonal elements in square matrix

    def test_det(self):
        a = np.asarray([[1,2],[3,4]])
        self.assertAlmostEqual(-2,np.linalg.det(a))

    def test_solve(self):
        a = np.asarray([[1,2],[3,4]])
        y = np.asarray([17,39])
        x = np.linalg.solve(a,y)
        npt.assert_allclose([5.0,6.0],x)

    def test_sum(self):
        # ------------------ one dimension
        a = np.asarray([5,4,15])
        self.assertEqual(24,np.sum(a))
        self.assertEqual(24,a.sum())

        # ------------------ two dimension
        m = np.asarray([[1,2,3],
                        [4,5,6]])

        # without axis, return a number representing the sum of all elements
        self.assertEqual(21,m.sum())

        sumEachColumns = m.sum(0)# 0-th axis, view from horizontal direction, sum each column
        npt.assert_equal([5,7,9],sumEachColumns)
        
        sumEachRows = m.sum(1)# 1-th axis, view from vertical diretion, sum each row
        npt.assert_equal([6,15],sumEachRows)

    def test_max_min(self):
        m = np.asarray([[1,2,3],
                        [4,5,6]])

        self.assertEqual(6,m.max())
        self.assertEqual(1,m.min())

        npt.assert_equal([4,5,6],m.max(0)) # max each column
        npt.assert_equal([1,4],m.min(1)) # min each row

    def test_argmax_argmin(self):
        m = np.asarray([[9, 1, 6],       
                        [3, 8, 5]])
        # if not specify axis, then the whole array is flatten, and return the
        # index
        self.assertEqual(1,m.argmin())
        self.assertEqual(0,m.argmax())

        npt.assert_equal([0,1,0],m.argmax(0))
        npt.assert_equal([1,0],m.argmin(1))

    def test_demo_ufunc(self):
        """
        A universal function, or ufunc, is a function that performs elementwise operations on data in ndarrays
        """
        a = [0,math.pi / 2.0]
        npt.assert_allclose([0,1],np.sin(a),atol=1e-6)
        npt.assert_allclose([1,0],np.cos(a),atol=1e-6)

    def test_elementwise_calculate(self):
        """
        numpy operations are usually done element-by-element
        """
        a = np.asarray([1,2,3])
        b = [4,5,6]
        npt.assert_equal([5,7,9],a + b)
        npt.assert_equal([4,10,18],a * b)# element-by-element multiply
        npt.assert_equal([-3,-3,-3],a - b)

    def test_elementwise_max_min(self):
        a = [1,2,3]
        b = [-1,6,1]
        npt.assert_equal([1,6,3], np.maximum(a,b))
        npt.assert_equal([-1,2,1], np.minimum(a,b))


    def test_elementwise_matrix_multiply(self):
        m1 = np.asarray([[1,1],         
                         [0,1]])
        m2 = np.asarray([[2,0],             
                         [3,4]])

        # ------------ element-by-element multiply
        npt.assert_equal([[2, 0],       
                          [0, 4]],m1 * m2)

        # ------------ matrix multiply
        npt.assert_equal([[5, 4],       
                          [3, 4]],np.dot(m1,m2))

    def test_norm(self):
        """just the length of the vector, sqrt(x*x + y*y + z*z)"""
        a = np.array([1,2,3,4])
        self.assertAlmostEqual(math.sqrt(30),np.linalg.norm(a))

    def test_digitize(self):
        a = [1.1,3.8,4.5,2.6,7,8.9,10.1]
        edges = np.arange(1,11)
        npt.assert_equal([1,  3,  4,  2,  7,  8, 10], np.digitize(a,edges))


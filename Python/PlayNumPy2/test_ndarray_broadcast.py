
import unittest
import numpy as np
import numpy.testing as npt

class NdArrayBroadcastTest(unittest.TestCase):

    def test_demo1(self):
        a = np.arange(1,4).reshape((3,1))
        b = np.arange(1,3)
        
        # a is repeated in horizontal direction, enlarge to 3*2 matrix
        # b is repeated in vertical direction, enlarge to 3*2 matrix
        # and then add together
        c = a + b
        npt.assert_array_equal([[2, 3],
                                 [3, 4],
                                 [4, 5]],c)

    def test_set_by_row_col(self):
        a = np.arange(1,7).reshape((3,2))
        
        # broadcast as rows
        a[:] = [6,9]
        npt.assert_equal([[6, 9],       
                          [6, 9],       
                          [6, 9]],a)

        # broadcast as columns
        content = np.asarray([6,8,9])
        with self.assertRaises(ValueError) :        
            a[:] = content # incompatile, cannot be broadcast

        a[:] = content[:,np.newaxis] # transform into column vector
        npt.assert_equal([[6, 6],        
                          [8, 8],       
                          [9, 9]],a)

    def test_normalize_sample(self):
        a = np.asarray([[1,4],
                        [5,2]])
        
        colmean = a.mean(0)
        npt.assert_equal([3,3],colmean)
        npt.assert_equal([[-2,1],[2,-1]],a - colmean)
        
        colmax = a.max(0).astype(np.float)
        npt.assert_allclose([[0.2,1],[1,0.5]],a / colmax)

    def test_remove_mean_sample(self):
        a = np.arange(1,7).reshape(3,2)

        colmean = a.mean(0)
        npt.assert_allclose([3,4],colmean)

        rowmean = a.mean(1)
        npt.assert_allclose([1.5,  3.5,  5.5],rowmean)

        # ----------- broadcast to remove column mean
        npt.assert_allclose([0,0],np.mean(a - colmean,0))# colmean is a 1d array, broadcast vertically

        # ----------- broadcast to remove row mean
        with self.assertRaises(ValueError):        # cannot be broadcasted
            a - rowmean

        npt.assert_allclose([[-0.5,  0.5],       
                             [-0.5,  0.5],       
                             [-0.5,  0.5]], a - rowmean[:,np.newaxis])

    def test_cannot_broadcast(self):
        """
        it ISN'T enough that: longer's size is multiple times of the shorter size
        the shorter size must be ONE
        """
        a = np.asarray([1,2,3,4])
        b = np.asarray([5,6])
        with self.assertRaises(ValueError):
            a + b
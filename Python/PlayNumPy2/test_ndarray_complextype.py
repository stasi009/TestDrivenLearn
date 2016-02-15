
import unittest
import numpy as np
import numpy.testing as npt

class NdArrayComplextypeTest(unittest.TestCase):

    def test_demo1(self):
        c = np.asarray( [ [1,2], [3,4] ], dtype=complex )
        expected = np.asarray([  [ 1.+0.j,  2.+0.j],                         
                                 [ 3.+0.j,  4.+0.j]  ])
        npt.assert_array_equal(expected,c)
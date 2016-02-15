
import unittest
import cStringIO
import numpy as np
import numpy.testing as npt

class NumPyFileIOTest(unittest.TestCase):

    def test_loadtxt1(self):
        f = cStringIO.StringIO("1,       2\n3,            4")# can automatically deal with space

        a = np.loadtxt(f,delimiter=",")
        self.assertEqual(np.float,a.dtype)
        
        npt.assert_allclose([[1.0,2.0],                                           
                              [3.0,4.0]],a)

    def test_savetxt1(self):
        f = cStringIO.StringIO()

        a = np.asarray([[1.111111111,2.222222,3.3333333],                        
                        [4.44444,5.555555,6.666666666]])

        np.savetxt(f,a,fmt="%3.2f",delimiter=",")

        self.assertEqual("1.11,2.22,3.33\n4.44,5.56,6.67\n",f.getvalue())
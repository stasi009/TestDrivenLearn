
import unittest
import numpy as np
import numpy.testing as npt
import pandas as pd
import pandas.util.testing as pdt
import test_testing as tt

class SetTest(unittest.TestCase):

    def test_unique(self):
        s = pd.Series(['c', 'a', 'd', 'a', 'a', 'b', 'b', 'c', 'c'])
        npt.assert_equal(['c', 'a', 'd', 'b'], s.unique())

    def test_value_counts(self):
        s = pd.Series(['c', 'a', 'd', 'a', 'a', 'b', 'b', 'c', 'c'])
        actual = s.value_counts()
        expected = pd.Series({"a":3,"b":2,"c":3,"d":1})
        tt.ignore_order_assert_series_equal(expected,actual)

    def test_isin(self):
        s = pd.Series(['c', 'a', 'd', 'a', 'a', 'b', 'b', 'c', 'c'])
        mask = s.isin(['b','c'])
        npt.assert_array_equal([True, False, False, False, False,  True,  True,  True,  True],mask.values)
        npt.assert_array_equal(['c','b', 'b', 'c', 'c'], s[mask])

    def test_build_histogram(self):
        df = pd.DataFrame({'a': [1, 3, 4, 3, 4],          
                           'b': [2, 3, 1, 2, 3],                                
                           'c': [1, 5, 2, 4, 4]},index=["r%d" % i for i in xrange(1,6)])
        histogram = df.apply(pd.value_counts).fillna(0)

        expected = pd.DataFrame([[1.,  1.,  1.],        
                                 [0.,  2.,  1.],       
                                 [2.,  2.,  0.],       
                                 [2.,  0.,  2.],       
                                 [0.,  0.,  1.]],columns=list("abc"),index=[1,2,3,4,5])
        pdt.assert_frame_equal(expected,histogram)


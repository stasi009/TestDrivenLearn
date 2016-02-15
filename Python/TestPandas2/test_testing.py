
import unittest
import numpy as np
import numpy.testing as npt
import pandas as pd
import pandas.util.testing as pdt

def ensure_frame_equal(actual_frame,expect_values,expect_columns,expect_index):
    npt.assert_allclose(expect_values,actual_frame.values)
    npt.assert_array_equal(expect_columns,actual_frame.columns)
    npt.assert_array_equal(expect_index,actual_frame.index)

def ignore_order_assert_series_equal(expected,actual, use_close=False):
    assert (isinstance(actual, pd.Series) and isinstance(expected, pd.Series)), 'Inputs must both be pandas Series.'

    if use_close: comp = npt.assert_allclose
    else: comp = npt.assert_equal

    for j, exp_item in expected.iteritems():
        assert j in actual.index, 'Expected column {!r} not found.'.format(j)
        act_item = actual[j]

        try:
            comp(act_item, exp_item)
        except AssertionError as e:
            raise AssertionError(e.message + '\n\nColumn: {!r}\nRow: {!r}'.format(j, i))


def ignore_order_assert_frames_equal(expected,actual, use_close=False):
    """
    Compare DataFrame items by index and column and
    raise AssertionError if any item is not equal.

    Ordering is unimportant, items are compared only by label.
    NaN and infinite values are supported.
    
    Parameters
    ----------
    actual : pandas.DataFrame
    expected : pandas.DataFrame
    use_close : bool, optional
        If True, use numpy.testing.assert_allclose instead of numpy.testing.assert_equal.
    """
    assert (isinstance(actual, pd.DataFrame) and isinstance(expected, pd.DataFrame)), 'Inputs must both be pandas DataFrames.'

    for i, exp_row in expected.iterrows():
        assert i in actual.index, 'Expected row {!r} not found.'.format(i)
        act_row = actual.loc[i]

        ignore_order_assert_series_equal(exp_row,act_row,use_close)

class TestingTest(unittest.TestCase):

    def test_assert_series_equal(self):
        s1 = pd.Series([1,2,3,4],index= ["a","b","c","d"])
        s2 = pd.Series({"d":4,"b":2,"a":1,"c":3})
        pdt.assert_series_equal(s1,s2)

        s3 = pd.Series([2,1,4,3],index= ["b","a","d","c"])
        with self.assertRaises(AssertionError): 
            pdt.assert_series_equal(s1,s3) # order matters

        # if ignore order, they are equal
        ignore_order_assert_series_equal(s1,s3)

    def test_assert_frame_equal(self):
        f1 = pd.DataFrame(np.arange(1,5).reshape((2,2)),index = ["r1","r2"],columns=["c1","c2"])
        self.assertEqual(np.int32,f1.dtypes.c1)

        f2 = pd.DataFrame({"c1":{"r1":1,"r2":3},
                           "c2":{"r1":2,"r2":4}})
        self.assertEqual(np.int64,f2.dtypes.c1)

        with self.assertRaises(AssertionError):
            # below test fails, because the types don't match
            pdt.assert_frame_equal(f1,f2)

        # we have to ignore the type checking
        pdt.assert_frame_equal(f1,f2,check_dtype=False)

    def test_bydefault_use_allclose(self):
        s1 = pd.Series([1, 2, 3], dtype='int')
        
        s2 = s1 + 0.0000000000000000001
        self.assertEqual(np.float64,s2.dtype)

        with self.assertRaises(AssertionError):
            pdt.assert_series_equal(s1,s2)

        pdt.assert_series_equal(s1,s2,check_dtype=False)

    def test_ignore_order_assert_frames_equal(self):
        expected = pd.DataFrame({'a': [1, np.nan, 3],                          
                                 'b': [4, 5, 6]},                        
                                index=['x', 'y', 'z'])
        actual = pd.DataFrame([[4, 1],
                               [6, 3],
                               [5, np.nan]],                               
                              index=['x', 'z', 'y'],                              
                              columns=['b', 'a'])
        ignore_order_assert_frames_equal(expected,actual)

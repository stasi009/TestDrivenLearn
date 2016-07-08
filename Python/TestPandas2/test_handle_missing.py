
import unittest
import numpy as np
import numpy.testing as npt
import pandas as pd
import pandas.util.testing as pdt

class HandleMissingTest(unittest.TestCase):

    def test_sum_treat_nan_0(self):
        s = pd.Series([np.nan,np.nan])
        self.assertAlmostEqual(0, np.sum(s))

        # but other statistics will treat all NaN as nan
        self.assertTrue(np.isnan( np.mean(s) ))
        
    def test_exclude_nan(self):
        """
        I think it is a bug, although it claims if entire row/column is NaN, it will return NaN
        However, for sum, even for 'sum', it will treat all NaN as zero, and return 0.0
        """
        df = pd.DataFrame([[1,np.nan,3,np.nan],[np.nan,5,6,np.nan]],columns=list("abcd"),index=["r1","r2"])
        pdt.assert_series_equal(pd.Series([1.0,5.0,9.0,0],index=df.columns),df.sum())
        pdt.assert_series_equal(pd.Series([4.0,11.0],index=df.index), df.sum(axis=1))

    def test_check_null(self):
        # None is equivalent as NaN
        s = pd.Series([1,None,2,np.nan,3],index=list("abcde"))

        pdt.assert_series_equal(pd.Series([False,  True, False,  True, False],index=s.index),s.isnull())
        pdt.assert_series_equal(s.notnull(),-(s.isnull()))

        # bool index
        pdt.assert_series_equal(pd.Series([1.0,2.0,3.0],index=list("ace")),s[s.notnull()])

    def test_dropna_series(self):
        # since there is np.nan, so integers are casted to float
        s = pd.Series([1,None,2,np.nan,3],index=list("abcde"))

        expected = pd.Series([1.0,2.0,3.0],index=list("ace"))

        pdt.assert_series_equal(expected,s.dropna())
        pdt.assert_series_equal(expected,s[s.notnull()])

    def test_frame_dropna(self):
        """
        dropna by default drops any row containing a missing value
        """
        df = pd.DataFrame([[1., 6.5, 3.], 
                           [1., np.nan, np.nan], 
                           [np.nan, np.nan, np.nan], 
                           [np.nan, 6.5, 3.]],columns=list("abc"),index=["r%d" % i for i in xrange(1,5)])

        pdt.assert_frame_equal(pd.DataFrame([[1.0,6.5,3.0]],columns=df.columns,index=["r1"]), df.dropna())

        # how='all' will only drop rows that are all NA:
        pdt.assert_frame_equal(df.dropna(how="all"),pd.DataFrame([[1. ,  6.5,  3.],       
                                                                   [1. ,  np.nan,  np.nan],       
                                                                   [np.nan,  6.5,  3.]],columns=df.columns,index=["r1","r2","r4"]))

        # drop columns
        df.a = np.nan
        pdt.assert_frame_equal(df.dropna(axis=1,how="all"), pd.DataFrame([[6.5,  3.],       
                                                                           [np.nan,  np.nan],       
                                                                           [np.nan,  np.nan],       
                                                                           [6.5,  3.]],columns=list("bc"),index=df.index))

    def test_fillna(self):
        df = pd.DataFrame([[1., 6.5, 3.], 
                           [1., np.nan, np.nan], 
                           [np.nan, np.nan, np.nan], 
                           [np.nan, 6.5, 3.]],columns=list("abc"),index=["r%d" % i for i in xrange(1,5)])

        # fill NAN from all columns with the same number
        pdt.assert_frame_equal(pd.DataFrame([[1. ,  6.5,  3.],                            
                                             [1. , -1. , -1.],                                                                                   
                                             [-1. , -1. , -1.],                                                                                  
                                             [-1. ,  6.5,  3.]],columns=df.columns,index=df.index), df.fillna(-1))

        # fill different columns with different values
        pdt.assert_frame_equal(pd.DataFrame([[1. ,  6.5,  3.],       
                                             [1. , -1. ,  0.],       
                                             [np.nan, -1. ,  0.],       
                                             [np.nan,  6.5,  3.]],index=df.index,columns=df.columns),  df.fillna({"b":-1,"c":0}))

        # fill in place
        _ = df.fillna({"a":-999,"b":-1,"c":0},inplace=True)# although in place, still have return value, but itself
        pdt.assert_frame_equal(pd.DataFrame([[1. ,    6.5,    3.],       
                                             [1. ,   -1. ,    0.],       
                                             [-999. ,   -1. ,    0.],       
                                             [-999. ,    6.5,    3.]],index=df.index,columns=df.columns),  df)

    def test_fillna_with_mean(self):
        df = pd.DataFrame([[1,np.nan,3],
                           [np.nan,5,6],
                           [7,8,np.nan]],   columns = ["a","b","c"],    index = ["record1","record2","record3"])
        means = df.mean()
        df.fillna(means,inplace=True)
        pdt.assert_frame_equal(df,pd.DataFrame([[ 1. ,  6.5,  3. ],       
                                                [ 4. ,  5. ,  6. ],       
                                                [ 7. ,  8. ,  4.5]],columns=df.columns,index=df.index))


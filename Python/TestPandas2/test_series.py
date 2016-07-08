
import unittest
import numpy as np
import numpy.testing as npt
import pandas as pd
import pandas.util.testing as pdt
import test_testing as tt

class SeriesTest(unittest.TestCase):

    def test_create_demo1(self):
        arr = np.asarray([6,8,9,1])
        self.assertEqual("int32",arr.dtype.name)

        # !!!  WON'T sort according to index
        # values and indices are in the original order when they are passed in
        s = pd.Series([6,8,9,1],index=["d","a","c","b"])
        self.assertEqual("int64",s.dtype.name)# by default, stored as long type

        npt.assert_array_equal([6,8,9,1],s.values)
        npt.assert_array_equal(["d","a","c","b"],s.index)
        self.assertEqual(9,s["c"])
        self.assertEqual(8,s.a)# index become attribute

    def test_create_from_dict(self):
        adict = {"y":9.9,"x":6.8,"a":1,"b":3.14}
        s = pd.Series(adict)
        self.assertEqual(np.float,s.dtype)
        self.assertAlmostEqual(1.0,s["a"])
        self.assertAlmostEqual(3.14,s.b)

        # when create from dict, indices will be reordered
        npt.assert_array_equal(["a","b","x","y"],s.index)
        npt.assert_allclose([1,3.14,6.8,9.9],s.values)

        # can pass in index, not only reorder the index,
        # but missing index set to NaN
        s2 = pd.Series(adict,index=["z","y","x"])
        pdt.assert_series_equal(pd.Series([np.nan,9.9,6.8],
                                          index=["z","y","x"]),s2)

    def test_update(self):
        s = pd.Series([6,8,9,1],index=["a","b","c","d"])
        s[["c","a"]] = [-100,-200]
        pdt.assert_series_equal(pd.Series([-200,8,-100,1],
                                          s.index),s)

    def test_index_by_slice(self):
        s = pd.Series(range(10),index=list("abcdefghij"))
        pdt.assert_series_equal(pd.Series([1,2,3],index = list("bcd")), s[1:4])

    def test_index_by_name(self):
        s = pd.Series([6,8,9,1],index=["a","b","c","d"])

        # get
        self.assertEqual(1,s["d"])
        self.assertEqual(8,s.b)# index become attribute
        npt.assert_equal([9,6], (s[["c","a"]]).values)# pay attention CANNOT use tuple

        # set
        s[["b","d"]] = [-99,-66]
        pdt.assert_series_equal(pd.Series([6,-99,9,-66],
                                          index=["a","b","c","d"]),s)

        # !!!  return a copy
        sliced_copy = s[["a","c"]]
        pdt.assert_series_equal(pd.Series([6,9],  index=["a","c"]),sliced_copy)

        sliced_copy.a = -66
        pdt.assert_series_equal(pd.Series([-66,9],  index=["a","c"]),sliced_copy)

        # but the original series isn't changed
        pdt.assert_series_equal(pd.Series([6,-99,9,-66],
                                          index=["a","b","c","d"]),s)

    def test_bool_index(self):
        """
        bool index return a copy
        """
        s = pd.Series([6,8,9,1],index=["a","b","c","d"])

        sliced_copy = s[s % 2 == 0]
        pdt.assert_series_equal(sliced_copy,pd.Series([6,8],index=["a","b"]))

        sliced_copy.a = -100
        pdt.assert_series_equal(sliced_copy,pd.Series([-100,8],index=["a","b"]))

        # the original isn't changed
        pdt.assert_series_equal(pd.Series([6,8,9,1],index=["a","b","c","d"]),s)


    def test_key_contained(self):
        s = pd.Series([6,8,9,1],index=["a","b","c","d"])
        self.assertTrue("a" in s)
        self.assertFalse("x" in s)

    def test_check_null(self):
        s = pd.Series([np.NAN,1,4])
        npt.assert_equal([True,False,False],s.isnull().values)
        npt.assert_equal([False,True,True],s.notnull().values)

    def test_arithmetic(self):
        s1 = pd.Series({"a":1,"b":9,"c":6})
        s2 = pd.Series({"c":1,"b":9,"d":8})

        # align the series, values at the same index will be added together
        # non-overlapped will return NaN
        # returned indices is a union of input indices
        sum1 = s1 + s2
        pdt.assert_series_equal(pd.Series([np.NaN,18.0,7.0,np.NaN],index=["a","b","c","d"]),sum1)

        # ----------------- pass in fill_value for non-overlapped indices
        # then since "a" is missing in s2, so s2.a will be 0.0
        # also since "d" is missing in s1, so s1.d will be 0.0
        sum2 = s1.add(s2,fill_value=0)
        pdt.assert_series_equal(pd.Series([1,18.0,7.0,8.0],index=["a","b","c","d"]),sum2)

    def test_equal(self):
        s1 = pd.Series([1,2,3],index=["a","b","c"])
        s2 = pd.Series({"c":3,"a":1,"b":2})# when pass in dict, index will be reordered
        
        self.assertTrue(s1 is not s2)
        self.assertTrue(s1.equals(s2))
        with self.assertRaises(ValueError):         npt.assert_equal(s1,s2)# don't know why

        # index in different order
        s3 = pd.Series([3,2,1],index=["c","b","a"])
        self.assertFalse(s1.equals(s3))# order matters
        tt.ignore_order_assert_series_equal(s1,s3)

        # "==" is different from "equals", but returns an array which indicates
        # equality on each position
        npt.assert_equal([True,True,True],(s1 == s2).values)

    def test_name(self):
        """ both series and its index can have name """
        sdata = {'Ohio': 35000, 'Texas': 71000, 'Oregon': 16000, 'Utah': 5000}
        s = pd.Series(sdata)
        s.name = "population"
        s.index.name = "state"
        self.assertEqual("population",s.name)

    def test_elementwise_calculation(self):
        s1 = pd.Series([1,2,3],index=["a","b","c"])
        expected = pd.Series([2.718282,7.389056,20.085537],index=["a","b","c"])
        pdt.assert_series_equal(expected,np.exp(s1))

    def test_reindex(self):
        s1 = pd.Series([1,2,3],index=["a","b","c"])

        # ---------- subset and reorder
        s2 = s1.reindex(["c","a"])
        npt.assert_equal([3,1],s2.values)

        s2.c = -99
        npt.assert_equal([-99,1],s2.values)

        # original isn't affected
        npt.assert_equal([1,2,3],s1.values)

        # ---------- missing index will be filled with NaN
        s3 = s1.reindex(["a","d"])
        npt.assert_equal([1,np.nan],s3.values)

        # ---------- provide default value when index missing
        s4 = s1.reindex(["b","c","x"],fill_value=-9999)
        npt.assert_equal([2,3,-9999],s4.values)

    def test_drop(self):
        s1 = pd.Series([1,2,3],index=["a","b","c"])
        new_series = s1.drop(["a","c"])
        npt.assert_equal([2],new_series.values)
        npt.assert_equal([1,2,3],s1.values)

        # return a copy
        new_series.b = -999
        npt.assert_equal([-999],new_series.values)
        npt.assert_equal([1,2,3],s1.values) # original isn't changed

    def test_map(self):
        s1 = pd.Series([-1,-2,3],index=["a","b","c"])
        s2 = s1.map(lambda x:  abs(x)) 
        pdt.assert_series_equal(pd.Series([1,2,3],index=["a","b","c"]),s2)

    def test_sort_index(self):
        s = pd.Series(range(4), index=['d', 'c','a', 'b'])
        sorted = s.sort_index()
        pdt.assert_series_equal(pd.Series([2, 3, 1, 0],index=list("abcd")),sorted)

    def test_order(self):
        s = pd.Series([np.nan,4, 7, np.nan,-3, 2],index=list("abcdef"))
        ordered = s.order()

        # by default, NaN is put in last
        pdt.assert_series_equal(pd.Series([-3.,   2.,   4.,   7.,  np.nan,  np.nan],index=list("efbcad")),ordered)

        # by default, return a ordered new series, the original isn't changed
        pdt.assert_series_equal(pd.Series([np.nan,4, 7, np.nan,-3, 2],index=list("abcdef")),s)

        # !!!  sort in place
        s.order(ascending=False,inplace=True)
        pdt.assert_series_equal(pd.Series([7.,   4.,   2.,  -3.,  np.nan,  np.nan],index=list("cbfead")),s)



        















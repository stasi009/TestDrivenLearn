
import unittest
import numpy as np
import numpy.testing as npt
import pandas as pd
import pandas.util.testing as pdt
import test_testing as tt

class FrameUpdateTest(unittest.TestCase):

    def test_update_column(self):
        frame = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])
        npt.assert_equal([[1, 2, 3],       
                          [4, 5, 6],       
                          [7, 8, 9]],frame.values)

        # update by a series
        # !!! notice that, the new series not only update what it contains
        # !!! if one index is missing in the new series, its value will be NaN
        # !!! and this NaN will overwrite the original
        frame.c = pd.Series([-99,-66],index=["record3","record2"])
        npt.assert_allclose([[1, 2, np.nan],              
                             [4, 5, -66],                       
                             [7, 8, -99]],frame.values)

        # update by list or array
        frame.b = [-100,-200,-300]
        npt.assert_allclose([[1, -100, np.nan],              
                             [4, -200, -66],                       
                             [7, -300, -99]],frame.values)

    def test_update_row(self):
        frame = pd.DataFrame({"department":{"tom":"IT","mary":"HR"},
                              "age":{"tom":25,"mary":21}})

        frame.ix["mary"] = pd.Series([24,"Law"],index=["age","department"])

        expected = pd.DataFrame({"department":{"tom":"IT","mary":"Law"},                              
                                 "age":{"tom":25,"mary":24}})
        pdt.assert_frame_equal(expected,frame)

    def test_add_row(self):
        frame = pd.DataFrame({"department":{"tom":"IT","mary":"HR"},
                              "age":{"tom":25,"mary":21}})

        frame.ix["alice"] = pd.Series([24,"Law"],index=["age","department"])

        expected = pd.DataFrame({"department":{"tom":"IT","mary":"HR","alice":"Law"},
                              "age":{"tom":25,"mary":21,"alice":24}})

        tt.ignore_order_assert_frames_equal(expected,frame)


    def test_add_column(self):
        frame = pd.DataFrame({"department":{"tom":"IT","mary":"HR"},
                              "age":{"tom":25,"mary":21}})

        # !!! not add
        frame.office = pd.Series({"mary":"3A","tom":"4B","alice":"5c"})
        npt.assert_array_equal(["age","department"],frame.columns)

        # can add column by following way
        frame["office"] = pd.Series({"mary":"3A","tom":"4B","alice":"5c"})
        npt.assert_array_equal(["age","department","office"],frame.columns)

        expected = pd.DataFrame({"department":{"tom":"IT","mary":"HR"},                              
                                 "age":{"tom":25,"mary":21},
                                 "office":{"tom":"4B","mary":"3A"}})
        # !!! still no row for "alice"
        pdt.assert_frame_equal(expected,frame)

    def test_delete_column(self):
        frame = pd.DataFrame({"department":{"tom":"IT","mary":"HR"},
                              "age":{"tom":25,"mary":21}})
        del frame["department"]
        npt.assert_array_equal(["age"],frame.columns)

    def test_drop(self):
        df = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])

        # ------------ drop rows
        npt.assert_equal([[7, 8, 9]], df.drop(["record1","record2"]).values)

        # ------------ drop columns
        npt.assert_equal( [[1],       
                           [4],       
                           [7]],df.drop(["b","c"],axis=1).values)

        # ------------ return dropped frame, the original isn't affected
        npt.assert_equal([[1, 2, 3],       
                          [4, 5, 6],       
                          [7, 8, 9]],df.values)


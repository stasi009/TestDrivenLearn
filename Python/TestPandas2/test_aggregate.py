
import numpy as np
import numpy.testing as npt
import pandas as pd
import pandas.util.testing as pdt
import unittest

class AggregateTest(unittest.TestCase):

    def test_cut_sum(self):
        a = pd.Series(np.arange(1,11))
        cuts = pd.cut(a,[2,5,7,9])

        # cut may return Series or Categorical, depending on the inputs
        # here, "cuts" is just a series
        self.assertIsInstance(cuts,pd.Series)

        actual = a.groupby(cuts).sum()
        expected = pd.Series([12,13,17],index=["(2, 5]", "(5, 7]", "(7, 9]"])
        npt.assert_equal(expected.values,actual.values)

        # below test will fail, although the values are the same
        # however, their indices are different, one is CategoricalIndex, one is just strings
        # pdt.assert_series_equal(expected,actual,check_dtype=False)

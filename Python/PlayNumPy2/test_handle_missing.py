
import unittest
import numpy as np
import pandas as pd

class HandleMissingTest(unittest.TestCase):

    def test_nan_mean(self):
        """
        in new version, skipna is removed, so there are two version of mean
        one can handle missing value, the other cannot
        """
        a = np.asarray([1,2,np.nan,3])
        np.isnan( np.mean(a) )# mean take NaN into account
        self.assertAlmostEqual(2, np.nanmean(a))# nanmean ignore nan


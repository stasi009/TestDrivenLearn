
#include "gtest/gtest.h"

TEST(CastingTest,OldCStyle) {
    int n = atoi("9");
    ASSERT_EQ(n,9);

    float fv = atof("9.9");
    ASSERT_FLOAT_EQ(fv,9.9000000009);// almost equal by default precision
}
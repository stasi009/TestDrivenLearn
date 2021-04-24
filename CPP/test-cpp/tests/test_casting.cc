
#include "gtest/gtest.h"

TEST(CastingTest, OldCStyle)
{
    int n = atoi("9");
    ASSERT_EQ(n, 9);

    float fv = atof("9.9");
    ASSERT_FLOAT_EQ(fv, 9.9000000009); // almost equal by default precision
}

TEST(CastingTest, StringToValue)
{
    auto s1 = std::string("99");
    int n = std::stoi(s1);
    ASSERT_EQ(n, 99);

    auto s2 = std::string("9.9");
    float fv = std::stof(s2);
    ASSERT_FLOAT_EQ(fv, 9.9000000009); // almost equal by default precision
}

#include "gtest/gtest.h"

struct MyStruct
{
    MyStruct(int a, float b) : a(a), b(b) {}
    int a;
    float b;
};

TEST(StructTest, Construct)
{
    auto s1 = MyStruct(1, 9.9);
    ASSERT_EQ(s1.a, 1);
    ASSERT_FLOAT_EQ(s1.b,9.8999999999999999);
}

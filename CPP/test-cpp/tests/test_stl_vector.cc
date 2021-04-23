
#include <vector>
#include "gtest/gtest.h"

TEST(VectorTest, Constructor)
{
    // --------- 初始化一个长度为0的vector
    std::vector<int> vec1;
    ASSERT_EQ(vec1.size(), 0);

    // --------- 初始化为指定长度，然后用指定值填充
    std::vector<int> vec2(3, 8);
    ASSERT_EQ(vec2.size(), 3);
    for (int x : vec2)
    {
        ASSERT_EQ(x, 8);
    }

    // --------- initializer_list
    std::vector<int> vec3 = {11, 22, 33};
    ASSERT_EQ(vec3[0], 11);
    ASSERT_EQ(vec3[1], 22);
    ASSERT_EQ(vec3[2], 33);
}
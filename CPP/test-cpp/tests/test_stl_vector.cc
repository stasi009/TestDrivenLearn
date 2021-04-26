
#include <iostream>
#include <vector>
#include <iterator>
#include "gtest/gtest.h"

using namespace ::std;

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

    // another syntax to construct via initializer_list
    auto vec4 = std::vector<int>{88, 99};
    ASSERT_EQ(vec4[0], 88);
}

TEST(VectorTest, AccessByIndex)
{
    // ------- read
    std::vector<int> vec1 = {11, 22, 33};
    ASSERT_EQ(vec1[0], 11);
    ASSERT_EQ(vec1[1], 22);
    ASSERT_EQ(vec1[2], 33);

    // ------- validate input index
    // !!! 以下代码能够运行，但是行为是不可预测的
    // 因为，[]不对访问位置进行检查
    // int x = vec1[99];

    ASSERT_THROW({ int y = vec1.at(99); }, std::out_of_range);

    // ------- write
    vec1[1] = 999;
    vec1.at(2) = 888;

    ASSERT_EQ(vec1[0], 11);
    ASSERT_EQ(vec1[1], 999);
    ASSERT_EQ(vec1[2], 888);
}

TEST(VectorTest, Equal)
{
    std::vector<int> vec = {11, 22, 33};

    // write by iterator
    for (std::vector<int>::iterator iter = begin(vec); iter != end(vec); ++iter)
    {
        // *iter can be both read and written
        // change in place
        *iter *= 2;
    }

    std::vector<int> expected = {22, 44, 66};
    ASSERT_TRUE(vec == expected);
}

TEST(VectorTest, LoopByIterator)
{
    std::vector<int> vec = {11, 22, 33};

    // read by iterator
    int index = 0;
    for (std::vector<int>::const_iterator citer = cbegin(vec); citer != cend(vec); ++citer)
    {
        ASSERT_EQ(*citer, vec[index]);
        ++index;
    }

    // write by iterator
    for (std::vector<int>::iterator iter = begin(vec); iter != end(vec); ++iter)
    {
        // *iter can be both read and written
        // change in place
        *iter *= 2;
    }
    ASSERT_TRUE(vec == (std::vector<int>{22, 44, 66}));
}

TEST(VectorTest, Swap)
{
    std::vector<int> vec1 = {11, 22, 33};
    std::vector<int> vec2 = {99, 88};

    // swap the contents of two vectors in constant time.
    vec1.swap(vec2);

    ASSERT_EQ(vec1, (std::vector<int>{99, 88}));
    ASSERT_EQ(vec2, (std::vector<int>{11, 22, 33}));
}
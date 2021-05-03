
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
    ASSERT_TRUE(vec1.empty());

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

    // random access by iterator
    vector<int>::const_iterator citer = cbegin(vec);
    citer += 2;
    ASSERT_EQ(*citer, 33);

    // read by iterator
    int index = 0;
    for (std::vector<int>::const_iterator citer = cbegin(vec); citer != cend(vec); ++citer)
    {
        ASSERT_EQ(*citer, vec[index]);
        ++index;
    }

    // in-place change by iterator
    for (std::vector<int>::iterator iter = begin(vec); iter != end(vec); ++iter)
    {
        // *iter can be both read and written
        // change in place
        *iter *= 2;
    }
    ASSERT_TRUE(vec == (std::vector<int>{22, 44, 66}));
}

TEST(VectorTest, LoopAndChange)
{
    std::vector<string> vec = {"a", "b", "c"};

    // --------- loop change by iterator
    for (auto it = begin(vec); it != end(vec); ++it)
    {
        it->append("_x");
    }
    ASSERT_EQ(vec, (std::vector<string>{"a_x", "b_x", "c_x"}));

    // --------- loop change in range-based loop
    for (auto &s : vec)
    {
        s.append("_y");
    }
    ASSERT_EQ(vec, (std::vector<string>{"a_x_y", "b_x_y", "c_x_y"}));
}

TEST(VectorTest, RangeBasedLoop)
{
    std::vector<int> vec = {11, 22, 33};

    // you can also use const auto& to perform ready-only access
    for (auto &n : vec)
    {
        n *= 2; // change in place
    }

    ASSERT_EQ(vec, (std::vector<int>{22, 44, 66}));
}

// Swap内部用的是Move Semantics，所以是constant time complexity
TEST(VectorTest, Swap)
{
    std::vector<int> vec1 = {11, 22, 33};
    std::vector<int> vec2 = {99, 88};

    // swap the contents of two vectors in constant time.
    vec1.swap(vec2);

    ASSERT_EQ(vec1, (std::vector<int>{99, 88}));
    ASSERT_EQ(vec2, (std::vector<int>{11, 22, 33}));
}

TEST(VectorTest, AssignOperator)
{
    auto vec1 = std::vector<int>{1, 2, 3};
    auto vec2 = std::vector<int>{8, 9};

    // assign operator makes a copy
    vec2 = vec1;
    ASSERT_EQ(vec2.size(), 3);
    ASSERT_EQ(vec2[0], 1);

    // copy, not reference
    vec2[0] = -99;
    ASSERT_EQ(vec1[0], 1); // not affect source which is copied from
}

TEST(VectorTest, PushAndPop)
{
    // push
    auto vec1 = std::vector<int>();
    vec1.push_back(9);
    vec1.push_back(8);
    vec1.push_back(6);
    ASSERT_EQ(vec1, (std::vector<int>{9, 8, 6}));

    // pop
    // pop_back() does not return the element that it removed.
    // If you want that element, you must first retrieve it with back().
    vec1.pop_back();
    ASSERT_EQ(vec1, (std::vector<int>{9, 8}));
}

TEST(VectorTest, MoveSemantics)
{
    auto vec1 = vector<string>();

    // triggers a call to the move version because the call to the string constructor results in a temporary object
    vec1.push_back((string(5, 'a')));

    // 'move' explicitly saying that myElement should be moved into the vector.
    // Note that after this call, myElement is in a valid but otherwise indeterminate state.
    auto s = string("xyz");
    vec1.push_back(move(s));

    ASSERT_EQ(vec1, (vector<string>{"aaaaa", "xyz"}));
}

TEST(VectorTest, Emplace)
{
    auto vec1 = vector<string>();

    // Emplace means “to put into place.” An example is emplace_back() on a vector object, which does not copy or move anything.
    // Instead, it makes space in the container, call the constructor, constructs the object in place,
    vec1.emplace_back(3, 'x');

    // above code is equivalent to following code
    // invoke the move-version, since the argument is a temporary object
    vec1.push_back(string(5, 'y'));

    ASSERT_EQ(vec1, (vector<string>{"xxx", "yyyyy"}));
}

TEST(VectorTest, ReserveAndCapacity)
{
    auto vec1 = vector<string>();
    ASSERT_EQ(vec1.size(), 0);

    vec1.reserve(10);
    ASSERT_EQ(vec1.size(), 0);
    ASSERT_EQ(vec1.capacity(), 10);
}
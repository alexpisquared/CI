#pragma once
#ifndef MIXED_MODE_MULTIPLY_HPP
#define MIXED_MODE_MULTIPLY_HPP

extern "C"
{
    __declspec(dllexport) int __stdcall mixed_mode_multiply(int a, int b) {
        int c = a * b;
        c = c * c;
        //__debugbreak();
        return c;
    }
}
#endif
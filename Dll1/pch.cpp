#include "pch.h"
#include "mkl.h"
#include <string.h>
#include <time.h>
#include <stdio.h>
#include <cmath>
#include <iostream>
#include <chrono>
#include <ctime> 

extern "C" _declspec(dllexport) int MKL_Function(int n, double* arr, float* arr_floats, float* results_time, int curr_function, double* res_ha, double* res_ep, float* res_ha_floats, float* res_ep_floats)
{
	try
	{
		MKL_INT64 mode1 = VML_HA;
		MKL_INT64 mode2 = VML_EP;
		std::chrono::system_clock::time_point start1;
		std::chrono::system_clock::time_point end1;
		std::chrono::system_clock::time_point start2;
		std::chrono::system_clock::time_point end2;
		std::chrono::duration<double> between1;
		std::chrono::duration<double> between2;

		if (curr_function == 0)   // vmdLn
		{
			start1 = std::chrono::system_clock::now();
			vmdLn(n, arr, res_ha, mode1);
			end1 = std::chrono::system_clock::now();
			between1 = end1 - start1;   //время вычисления vmdLn с точностью HA
			results_time[0] = between1.count();

			start2 = std::chrono::system_clock::now();
			vmdLn(n, arr, res_ep, mode2);
			end2 = std::chrono::system_clock::now();
			between2 = end2 - start2;   //время вычисления vmdLn с точностью EP
			results_time[1] = between2.count();

			results_time[2] = between2.count() / between1.count();  // отношение времени EP к HA

		}

		else if (curr_function == 1)   // vmsLn
		{
			start1 = std::chrono::system_clock::now();
			vmsLn(n, arr_floats, res_ha_floats, mode1);
			end1 = std::chrono::system_clock::now();
			between1 = end1 - start1;   //время вычисления vmsLn с точностью HA
			results_time[0] = between1.count();

			start2 = std::chrono::system_clock::now();
			vmsLn(n, arr_floats, res_ep_floats, mode2);
			end2 = std::chrono::system_clock::now();
			between2 = end2 - start2;   //время вычисления vmsLn с точностью EP
			results_time[1] = between2.count();

			results_time[2] = between2.count() / between1.count();   // отношение времени EP к HA

		}

		else if (curr_function == 2)  // vmdLGamma
		{
			start1 = std::chrono::system_clock::now();
			vmdLGamma(n, arr, res_ha, mode1);
			end1 = std::chrono::system_clock::now();
			between1 = end1 - start1;   //время вычисления vmdLGamma с точностью HA
			results_time[0] = between1.count();

			start2 = std::chrono::system_clock::now();
			vmdLGamma(n, arr, res_ep, mode2);
			end2 = std::chrono::system_clock::now();
			between2 = end2 - start2;   //время вычисления vmdLGamma с точностью EP
			results_time[1] = between2.count();

			results_time[2] = between2.count() / between1.count();  // отношение времени EP к HA
		}

		else if (curr_function == 3)   // vmsLGamma
		{
			start1 = std::chrono::system_clock::now();
			vmsLGamma(n, arr_floats, res_ha_floats, mode1);
			end1 = std::chrono::system_clock::now();
			between1 = end1 - start1;   //время вычисления vmsLGamma с точностью HA
			results_time[0] = between1.count();

			start2 = std::chrono::system_clock::now();
			vmsLGamma(n, arr_floats, res_ep_floats, mode2);
			end2 = std::chrono::system_clock::now();
			between2 = end2 - start2;   //время вычисления vmsLGamma с точностью EP
			results_time[1] = between2.count();

			results_time[2] = between2.count() / between1.count();   // отношение времени EP к HA

		}

		return 0;
	}
	catch (...)
	{
		return -1;
	}
}

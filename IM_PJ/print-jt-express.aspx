<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="print-jt-express.aspx.cs" Inherits="IM_PJ.print_jt_express" %>

<!DOCTYPE html public "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>In vận đơn J&T Express</title>
    <link rel="stylesheet" href="App_Themes/Ann/css/pages/print-jt-express/print-jt-express.css?v=202109291533" />
</head>
<body>
    <div id="page_1">
        <div id="p1dimg1">
            <img id="p1img1" alt="J&T Express logo" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAgAAAALsCAIAAAAanJhUAAAV8UlEQVR4nO3dTcgcdYLH8X6yTzDoPiZDjBsyo4dlYBlfDkMzjDiXYRjGl4uL7CAeRMRB5hAMzjpeRAkre1FRFA+uO7LIHiS4yHrRGRlkLgk7bJo9+MbCMAedCVljMC8kJBiSPZRbW1Z1V1f30y/Pk9/nQ5Dn6a6uqqcfn/+3u6q6auXSpUuDwaDf7/cAyFAM+1uWvRoALIcAAIQSAIBQAgAQSgAAQgkAQCgBAAi1uuwV+MrKykrxxaVLl4pviy/Ku8pvqw8pJy5vbN5S3l7Opznz2pRDV2nomtRuqT5k1MNrt7SscHMpzTkMnaZlic3lNu9qWclRyx26wtWfpcuPACyYdwAAoTbKOwBgCkd37WmfYPexI5M+pKP5zXmdmivGKN4BwGZ15uVXJn3I/MZoo/9mJACwWZ1+Yn/7BDvfe3dOi96Y4+zGXKuNzCYguGxtvfmmxSxo3i//V2+84Zrf/Xaui8gkAMBkFv9C+8KHH7U3xmv/6QgApLh48mTHKavjaW3kvfKhn9UmXvrWf6P/1AQANqUpht0z//TPY6e5+sXnr7z3nuotxfBaLu7qf/yHSZc7NSP7vAkAXJ6ao+faY4+eeea59kfVRv/q3I7u2rPIQz+33nrL8bvubp9m51tvzmnpIQQA+Mqkr7jHDtDr8eWh/2ifwPuD9XMYKNBJc8AdO0bPj9F/JgQANp9Tjz+57FXo/c9f/82yFm30nxWbgGDzOfvKr9onuPYPHw+9vdiUP5N1+Ks//vekD5nixBXMlXcAcBnasn37sleBTUAAIMuGfZXd/IQB8yYAwFfm+pGusTNf5CcMKAgAbDLr35LeMsHSP9bLIgkAxGkf5TUghwBAli7je8s00+Xhwieftk+w7Z6fTjFb1kkAIEj34bu9AcW/7sv9vP/99gl2vPRC97kxKwIAm8l6zr4w6Yv3E3v3jZ1h8e/L9z+Yeq1YIgGAzWTqM+RMsenm3IE3Oj7q+I9+MunM2QgEAC5/69mvW33snPYPr6ytzWO2jCUAcJnrMmq3Hzk6dg7tn+Ea+/ApzirBTDgXEEQrh/720wQd3bXn6hefH3XvXD/D1bJW1XWuXTe4+qjyZ6zNamj2zh889MXf/t3YydrXqvz6wiefVneAX3H7bd/4139pn9UiP6rtHQBsGlN8BGyiS+m2Dz2nHn6kfenz0HLO0WJt157aX3x74cOPWibrDXsqmscynX39QG30HzpZR0d37akd/nT+178ZO+dFfg5DACDU0OF+ipefcz2E/9Lp0+0TXPXzh5o3TjSGViduiVzHfSEtvWmZ4dA5LIBNQMDXTHrK6PZD+Gd1Cuguk517+51td94xduNP88bPf/jj6uaj2uJafoTadqehDxk7qyWens87ALhsTb0xYed77852TeaqHEBP3P/g0NurvvHv/9a8cdTmo7GGjv4t69D9/cFiCABsDmOHjK233jKrZW29+aaOU260k0uXz9KoZ+OKH9y6wNXZKAP9KDYBwWVi51tvznBuXTYEjbruWHc7Xnu15d7zBw+VX096qMxEz8bUp0ftslZHd+1p3l59eosvlpJS7wDgstU+QK//xenY646NXcS2O+9oubd5QM4o3Q9nunjyZK/D4TfN8x2V8/z8hz+edH26HEe0lPcK3gHAEizmr73jAD3pwaPVyTbIJqD2Ff7ivgfKrz/79ndq9056+YSOOwyab6FqT9fuY0fOvf1Odb/F4p9PAYBFK16EbhzrPFHEAsas9kWMPWld7ej7jrMtjfoZO5aj+vQWxymV3267845aJxbcAJuAYNGaL0LXb9SosYDR5PTTz073wJaPFk/k3IE3ii/KHb8th9hX/42a4dgJJlKdT+04pXKCZZ0NSQBgoao7Nhdj/btq25155rmht48/g9C996x/6eVSdh87Mtvd4Iu0rLMhCQAsVPcdm7OyZfv2eb8PmMcujXVeuazLrtruiysvw3D29QNdZlW9QEJzT3Jzn/Cyjha1DwAWZ+zW6vnZfezIib37yq0lk7rql79Ye+zR9rPFzS8zteVeM/j96vXXffbd75W3lIteWVsrzh5R7qqd9GI1Q3+K8jIM1XNFNJ+N3ceOFBe/7HKBhI3wqWDvAGBxph5/2131y190mWzHSy/sPnZk0k/5FlvD1x57tDfutD8TvYy94vbbJlqNqtXrr+v1ehf/9OfmXc1tKeu5WM1En60rxu6Wi19OerjRAngHAAvScmLLdSpG54623nxTOdBcPHlyosMid7z0wvFP2y7vXh7lcvyuu9tHz+ZZkScy6mQ7tWlqh11OupSdb7056caZ5pE/W771zWv/6z9r05x6/Mmzr/xqPes2EyuXLl0aDAb9fn8piwdg8Yph3yYggFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIALM7pp58tzoV55uVXJnpg7QyaXS6yuEQbfPVKzgUEs7ERTu7YXfNikMUtV7/4/ExO0z9qiaXTT+w//cT+DfL8NH93O997d+vNNy1lZRZJAGAGyvPFb5ARbVLliermPfqXz09xS8eTSDdP6TzD5/nU409W51ms2PEf/WQ9i9gs/xvYBAQzUJwvfj1/9mVCxt51+ulnh058/K67m5crObF339CJayetLM6hX1v/llVqN+qB3c/NWZtDcUrnsU/vib37hm5ZOvv6gZafpTgrZznzKX6JHZ+o8wcPjZpy6O2nHn9y1C96otVr4WygMAPNLSqj7qpuaSm+Xr3xhvLqJVfcfltxnuTaXc2TDBeG3t4+cXPFamt4/K67y+uf1B449GdpWYfqZNf+4eMt27c3V2Blba04iX9zhbfeektxlcexy912z0+r11qoTdnyJDRn3r64Lj91c/rald/Lu84fPFS7QtwUv+jpFMO+TUAwM0P/yIs//uoosLK2Vt3ScuHDj8o/7/O//k11huVd1ZnXxoLyVf/Q4aBljCguntUca6rvZop7v7jvge6n7x+1xNroX/rLx/6++m0x6BcXKqh1qMW5A280h8jy8sujRtWqUWPr0NG/uMxy7a6hF6GsbemqfVuM/mODXfwgtcVVL4g2NZuAYEHKv/DaVataxujmXcUt1Y0A1YsUTqS6GqPGu+KLIktFabZ865vFXaeffrbLCo911c8f6lWenOIl/6hajDJ0udXhdbaqq1cG+Jrf/XbU9LV1GPpGocUVP7i1/LrcHVK7yMx0vAOAIbocw1f+6ZZ7UEf9MVe3AEw3HpWbtptvMsqZDx1QuowyHVepKE057px55rkujx3aibFrUjxq9cYbeo3wFM/2jtde7TjP6jPWvKTlF/c9UFv0RHune73eqYcfOfXwI8U1kzuu0qjVG3pXuRo7Xnv1xP0PFrtD1p7aX4Rz7ExKQ38c7wBgvYbuQa0a+vc5dItBoRyVSqef2F/99to/fFzboVpeYrdampZN4R11fJXaMtlfXHddy0NGKepSvKauhad4tosLT5YH8Iy19tT+3ceO7HjphdrtxZub6a4cWX2GixXu/d/PNdHFhJtzG/q723bnHf8fyK///zA17wBgiBluNxg10lX37va+PoY2R6WxK1bddVybfrmfSLry3nuaG6lO7N1XfLHO57l2AM9Qky6ieLrKNxxVF0+eHDX/cjdPcWOxIat59OpEqzf0dzd0cVM/jd4BwGwUf5Plv/LG4ovyNV3tT7q69aZFtRNfvv9BbRHnDx66ePJk7YOyxU7Cljlf+GTk5d2P7tpz9vUDxWOvfOhntbvOHzxU7s1uX+3aA8t/xRE7Ew1bXTLWshGs/aO5zV9cbQv7ubff+ey73/vs29/pfX0I7vV6Z15+ZdQboC5HrxbP54m9+6q/0GIzV+0XWnxbfJR67Gw78g4A5mXU69wv3/+g+inTjrsHyiMdi2Gl+tjqoYTlTC7+6c/tc/68//1eY2NF+aqzeNle/UBseVe5uNre7FFaXslWf4qZ762t7R3p/qjmjSfuf3DoveWca5kcqrmDvfnrK1SPzmo+Ub1eb+2p/WMX14XPAcByzGnUgy6KYd8mIFiCYvS/ZvD7Za8I0QQAlmb1+uFHyMBiCABAKDuBYQls+mcj8A4AIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJwDQGg8GyVwFYkMv4710AAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQBo0+/3l70K8yIAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBqdTAY9Hq94r8A5Fjt9/uDwaDf7y97TTYTvQQuAzYBAYQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoASDIYDBY9irABrJy+PDhZa8DAIvW7/dX+/3+YDDo9/vLXhmYO/+rQ6F4N2wTEEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEGrl8OHDy14HABat3++v9vv9wWDQ7/eXvTIALMhgMOjZBAQQSwAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUKuDwaDX6xX/BSDHar/fHwwG/X5/2WsCwIIUL/ptAgIIJQAAoQQAIJQAAIQSAIBQAgAQSgAAQgkAQCgBAAglAAChBAAglAAAhBIAgFACABBKAABCCQBAKAEACCUAAKEEACCUAACEEgCAUAIAEEoAAEIJAEAoAQAIJQAAoQQAIJQAAIRaOXz48LLXAYAl+F8xAECYmT4eNgAAAABJRU5ErkJggg==" />
        </div>

        <div id="dclr"></div>

        <div id="id1_1">
            <table cellpadding="0" cellspacing="0" class="t0">
                <tr>
                    <td class="tr0 td0">
                        <p class="p0 ft0">
                            <asp:Literal ID="ltJtCode" runat="server"></asp:Literal>
                        </p>
                    </td>
                    <td class="tr0 td1">
                        <p class="p1 ft1">&nbsp;</p>
                    </td>
                    <td class="tr0 td2">
                        <p class="p1 ft1">&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td class="tr1 td3">
                        <p class="p2 ft0">Mã đơn đặt</p>
                    </td>
                    <td class="tr1 td4">
                        <p class="p3 ft2">
                            <asp:Literal ID="ltOrderIdHeader" runat="server"></asp:Literal>
                        </p>
                    </td>
                    <td class="tr1 td5">
                        <p class="p4 ft2">
                            <span class="ft0">Ngày gửi: </span>
                            <asp:Literal ID="ltSentDate" runat="server"></asp:Literal>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="tr2 td6">
                        <p class="p1 ft3">&nbsp;</p>
                    </td>
                    <td class="tr2 td7">
                        <p class="p1 ft3">&nbsp;</p>
                    </td>
                    <td class="tr2 td8">
                        <p class="p1 ft3">&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td class="tr3 td3">
                        <p class="p5 ft4">
                            Người gởi :
                            <asp:Literal ID="ltSenderName" runat="server"></asp:Literal>
                        </p>
                    </td>
                    <td class="tr3 td4">
                        <p class="p1 ft1">&nbsp;</p>
                    </td>
                    <td class="tr3 td5">
                        <p class="p6 ft4">
                            Người nhận :
                            <asp:Literal ID="ltReceiverName" runat="server"></asp:Literal>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="tr4 td3">
                        <p class="p7 ft4">
                            <asp:Literal ID="ltSenderPhone" runat="server"></asp:Literal>
                        </p>
                    </td>
                    <td class="tr4 td4">
                        <p class="p1 ft1">&nbsp;</p>
                    </td>
                    <td class="tr4 td5">
                        <p class="p5 ft4">
                            <asp:Literal ID="ltReceiverPhone" runat="server"></asp:Literal>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="tr5 td9">
                        <p class="p8 ft4">
                            Địa chỉ :
                            <asp:Literal ID="ltSenderAddressLine1" runat="server"></asp:Literal>
                        </p>
                    </td>
                    <td class="tr5 td5">
                        <p class="p6 ft4">
                            Địa chỉ :
                            <asp:Literal ID="ltReceiverAddressLine1" runat="server"></asp:Literal>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="tr5 td3">
                        <p class="p9 ft4">
                            <asp:Literal ID="ltSenderAddressLine2" runat="server"></asp:Literal>
                        </p>
                    </td>
                    <td class="tr5 td4">
                        <p class="p1 ft1">&nbsp;</p>
                    </td>
                    <td class="tr5 td5">
                        <p class="p10 ft4">
                            <asp:Literal ID="ltReceiverAddressLine2" runat="server"></asp:Literal>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="tr6 td6">
                        <p class="p9 ft4">
                            <asp:Literal ID="ltSenderAddressLine3" runat="server"></asp:Literal>
                        </p>
                    </td>
                    <td class="tr6 td7">
                        <p class="p1 ft1">&nbsp;</p>
                    </td>
                    <td class="tr6 td8">
                        <p class="p10 ft4">
                             <asp:Literal ID="ltReceiverAddressLine3" runat="server"></asp:Literal>
                        </p>
                    </td>
                </tr>
            </table>

            <table cellpadding="0" cellspacing="0" class="t1">
                <tr>
                    <td rowspan="3" class="tr7 td10">
                        <p class="p11 ft5">
                            <asp:Literal ID="ltPostalCode" runat="server"></asp:Literal>
                        </p>
                    </td>
                    <td class="tr8 td11">
                        <p class="p12 ft6">Số kiện</p>
                    </td>
                </tr>
                <tr>
                    <td class="tr9 td11">
                        <p class="p13 ft7">
                            Mã đơn KH:
                            <asp:Literal ID="ltOrderIdBody" runat="server"></asp:Literal>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="tr5 td11">
                        <p class="p14 ft8">
                            <asp:Literal ID="ltItemNumber" runat="server"></asp:Literal>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="tr10 td12">
                        <p class="p1 ft9">&nbsp;</p>
                    </td>
                    <td class="tr10 td13">
                        <p class="p1 ft9">&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td class="tr1 td10">
                        <p class="p15 ft10">
                            <asp:Literal ID="ltPostalBranchCodeLine1" runat="server"></asp:Literal>
                        </p>
                    </td>
                    <td class="tr1 td11">
                        <p class="p16 ft11">Nội dung</p>
                    </td>
                </tr>
                <tr>
                    <td class="tr6 td12">
                        <p class="p17 ft12">
                            <asp:Literal ID="ltPostalBranchCodeLine2" runat="server"></asp:Literal>
                        </p>
                    </td>
                    <td class="tr11 td11">
                        <p class="p18 ft2">
                            <asp:Literal ID="ltItemName" runat="server"></asp:Literal>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="tr3 td10">
                        <p class="p17 ft2">Loại vận chuyển</p>
                    </td>
                    <td class="tr3 td11">
                        <p class="p1 ft1">&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td class="tr1 td10">
                        <p class="p19 ft11">ET</p>
                    </td>
                    <td class="tr1 td11">
                        <p class="p1 ft1">&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td class="tr10 td12">
                        <p class="p1 ft9">&nbsp;</p>
                    </td>
                    <td class="tr12 td11">
                        <p class="p1 ft13">&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td class="tr13 td10">
                        <p class="p17 ft2">PTTT</p>
                    </td>
                    <td class="tr13 td11">
                        <p class="p1 ft1">&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td class="tr1 td10">
                        <p class="p19 ft11">PP_PM</p>
                    </td>
                    <td class="tr1 td11">
                        <p class="p1 ft1">&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td class="tr14 td12">
                        <p class="p1 ft14">&nbsp;</p>
                    </td>
                    <td class="tr10 td11">
                        <p class="p1 ft9">&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td class="tr3 td10">
                        <p class="p17 ft2">Vận phí</p>
                    </td>
                    <td class="tr3 td11">
                        <p class="p1 ft1">&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td class="tr15 td12">
                        <p class="p1 ft1">&nbsp;</p>
                    </td>
                    <td class="tr16 td11">
                        <p class="p1 ft1">&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td class="tr4 td10">
                        <p class="p20 ft4">Thu hộ COD</p>
                    </td>
                    <td class="tr4 td11">
                        <p class="p1 ft1">&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td class="tr1 td10">
                        <p class="p19 ft11"><asp:Literal ID="ltCod" runat="server"></asp:Literal></p>
                    </td>
                    <td class="tr1 td11">
                        <p class="p1 ft1">&nbsp;</p>
                    </td>
                </tr>
            </table>

            <p class="p21 ft2">Trọng lượng</p>
            <p class="p22 ft11">
                <span class="ft11">
                    <asp:Literal ID="ltWeight" runat="server"></asp:Literal>
                </span>
                <span class="ft15">KG</span>
            </p>

            <table cellpadding="0" cellspacing="0" class="t2">
                <tr>
                    <td class="tr3 td14">
                        <p class="p1 ft11">Người gởi ký tên</p>
                    </td>
                    <td class="tr3 td15">
                        <p class="p1 ft6">Người nhận ký tên</p>
                    </td>
                </tr>
            </table>

            <p class="p23 ft16">Xác nhận đã nhận được bưu kiện trong tình trạng tốt</p>
            <p class="p24 ft11">Ghi chú</p>
            <p class="p25 ft4">
                <asp:Literal ID="ltNote" runat="server"></asp:Literal></p>
        </div>

        <div id="id1_2">
            <p class="p26 ft8">Express Your Online Business - <span class="ft17">www.jtexpress.vn </span>- Hotline: <span class="ft17">1900 1088</span></p>
        </div>
    </div>
</body>
</html>

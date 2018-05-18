#include "embARC.h"

#include "board.h"
#include "dev_uart.h"
#include "dev_pinmux.h"

#include "vlsi_uart.h"
#include "vlsi_esp8266.h"

#include <stdio.h>
#include "embARC_debug.h"

#define WIFI_SSID   "\"VLSILAB_2G2\""
#define WIFI_PWD    "\"vlsi95514\""

char esp_data_cmd[] = "AT+CIPSEND=0,252\r\n";
static char http_get[] = "GET /";
static char http_IDP[] = "+IPD,";
/* static char http_html_header[100] = ""; */
static char http_html_header[60] = "HTTP/1.x 200 OK\r\nContent-type: text/html\r\n\r\n";
/* static char http_html_header[] = "HTTP/1.x 200 OK\r\nContent-type: application/octet-stream\r\n\r\n"; */
/* static char http_html_header[] = "HTTP/1.x 200 OK\r\nContent-type: txt/plain\r\n\r\n"; */
/* static char http_html_body_1[100] = "22"; */
/** static char send_buf[420] = {0}; */

/* extern float* pose; */

char http_sent[80];

int main(void)
{
    EMBARC_PRINTF("http_header size = %d\r\n", sizeof(http_html_header));
    char *conn_buf;
    char scan_result[1024];
    // set pin
    set_pmod_mux(0x151015);
    set_uart_map(0x9c);

    // uart init

    DEV_UART_PTR uart_obj = uart_get_dev(2);

    int opend = uart_obj->uart_open(USE_BAUD);
    if (opend == AT_OK) {
      EMBARC_PRINTF("uart2 init done!\r\n");
    }

    uart_obj->uart_control(UART_CMD_SET_RXCB, uart_cb);
    uart_obj->uart_control(UART_CMD_SET_RXINT, 1);
    uart_obj->uart_control(UART_CMD_ENA_DEV, NULL);
    
    clear_rx_buf(uart_obj);

    // esp8266
    int f;
    struct vlsi_esp *vlsi_esp8266;
    vlsi_esp8266_init(vlsi_esp8266, 115200);
    /* vlsi_esp8266_init(vlsi_esp8266, UART_BAUDRATE_230400); */
    /* vlsi_esp8266_set_baud(vlsi_esp8266, 230400); */
    f = vlsi_esp8266_test(vlsi_esp8266);
    EMBARC_PRINTF("test %d \r\n", f);
    board_delay_ms(1000, 1);

    /* vlsi_esp8266_set_baud(vlsi_esp8266, 0); */
    
    f = vlsi_esp8266_mode_set(vlsi_esp8266);
    EMBARC_PRINTF("mode set %d \r\n", f);
    board_delay_ms(1000, 1);

    f = vlsi_esp8266_connect(vlsi_esp8266, WIFI_SSID, WIFI_PWD);
    if (f == AT_OK) {
      EMBARC_PRINTF("connect OK \r\n");
    }
    board_delay_ms(1000, 1);

    f = vlsi_esp8266_server(vlsi_esp8266);
    EMBARC_PRINTF("server %d \r\n", f);
    board_delay_ms(1000, 1);

    f = vlsi_esp8266_get_ip(vlsi_esp8266);
    board_delay_ms(1000, 1);

    char idp;

    EMBARC_PRINTF("WiFi init done... \r\n");
    scan_result[0] = '\0';

    char* send_buf = R;
    while(1)
    {
      /* for (int i = 0; i < 4; ++i) { */
        /* for (int j = 0; j < 4; ++j) { */
          /* EMBARC_PRINTF("%d ", (int)(R[i*4+j]*1000)); */
          /* if (j == 4) { */
            /* EMBARC_PRINTF("\r\n"); */
          /* } */
        /* } */
          /* EMBARC_PRINTF("\r\n"); */
      /* } */
      /* EMBARC_PRINTF("-----------\r\n"); */

      /* scan_result[0] = '\0'; */
      vlsi_esp8266_read(vlsi_esp8266, scan_result, 1000);
      /* if (strstr(scan_result, http_get) != NULL) { */
      while (scan_result[0] != 0) {
        EMBARC_PRINTF("recieve = %s ", scan_result);
        // find who request
        idp = *(strstr(scan_result, http_IDP)+5);
        EMBARC_PRINTF("idp = %c \r\n", idp);

        /* vlsi_esp8266_send(vlsi_esp8266, http_html_header, idp); */
        /* board_delay_ms(100, 1); */
        for (int i = 0; i < 1; ++i) {
          vlsi_esp8266_send_datas(vlsi_esp8266, esp_data_cmd, sizeof(esp_data_cmd), send_buf, 252);
          /* vlsi_esp8266_send_pose(vlsi_esp8266, (send_buf+210*0), idp); */
          board_delay_ms(5, 1);
          /* vlsi_esp8266_send_pose(vlsi_esp8266, (send_buf+210*1), idp); */
          /* board_delay_ms(10, 1); */
          /* vlsi_esp8266_send_pose(vlsi_esp8266, (send_buf+105*2), idp); */
          /* board_delay_ms(10, 1); */
          /* vlsi_esp8266_send_pose(vlsi_esp8266, (send_buf+105*3), idp); */
          /* board_delay_ms(10, 1); */
        }

        /* vlsi_esp8266_close(vlsi_esp8266, idp); */

        EMBARC_PRINTF("send complete %c \r\n");
        /* for (int i = 0; i < 1024; ++i) { */
          /* scan_result[i] = 0; */
        /* } */
      }
      board_delay_ms(100, 1);
    }

    return E_OK;
}

using MauiTempoAgora.Models;
using MauiTempoAgora.Services;
using System;
using System.Diagnostics;

namespace MauiTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void ButtonPrevisao_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if(t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Por do Sol: {t.sunset} \n" +
                                         $"Temperatura Máxima: {t.tempmax} \n" +
                                         $"Temperatura Mínima: {t.tempmin} \n";

                        lbl_res.Text = dados_previsao;

                        try
                        {
                            await App.Db.Insert(t);
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Você é um imundo!", ex.Message, "OK");
                        }
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de previsão.";
                    }
                }
                else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void lst_historico_Refreshing(object sender, EventArgs e)
        {

        }

        private void lst_historico_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}

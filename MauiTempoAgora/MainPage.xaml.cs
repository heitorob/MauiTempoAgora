﻿using MauiTempoAgora.Models;
using MauiTempoAgora.Services;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MauiTempoAgora
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Tempo> lista = new ObservableCollection<Tempo>();

        public MainPage()
        {
            InitializeComponent();

            lst_historico.ItemsSource = lista;
        }

        protected async override void OnAppearing()
        {
            try
            {
                lista.Clear();
                List<Tempo> tmp = await App.Db.GetAll();
                tmp.ForEach(i => lista.Add(i));
                lbl_res.Text = "Insira a cidade para ver a previsão.";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void ButtonPrevisao_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        t.cidade = CapitalizarNome(txt_cidade.Text);
                        t.data = DateTime.Now;
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Pôr do Sol: {t.sunset} \n" +
                                         $"Temperatura Máxima: {t.tempmax} \n" +
                                         $"Temperatura Mínima: {t.tempmin} \n" +
                                         $"Descrição: {t.description}";

                        lbl_res.Text = dados_previsao;

                        try
                        {
                            await App.Db.Insert(t);
                            lista.Add(t);
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Erro", ex.Message, "OK");
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
                txt_cidade.Text = null;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string q = e.NewTextValue;

                lst_historico.IsRefreshing = true;

                lista.Clear();

                List<Tempo> tmp = await App.Db.Search(q);

                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
            finally
            {
                lst_historico.IsRefreshing = false;
            }
        }

        private async void lst_historico_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lista.Clear();

                List<Tempo> tmp = await App.Db.GetAll();

                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
            finally
            {
                lst_historico.IsRefreshing = false;
            }
        }

        private async void lst_historico_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                Tempo t = e.SelectedItem as Tempo;

                if (t == null)
                {
                    await DisplayAlert("Erro", "Nenhum registro selecionado.", "OK");
                    return;
                }

                bool resposta = await DisplayAlert("Apagar Registro", "Deseja realmente apagar este registro?", "Sim", "Não");
                if (!resposta) return;

                await App.Db.Delete(t.Id);

                try
                {
                    lista.Clear();
                    List<Tempo> tmp = await App.Db.GetAll();
                    tmp.ForEach(i => lista.Add(i));
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ops", ex.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        public static string CapitalizarNome(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return texto;

            string[] excecoes = { "de", "da", "do", "das", "dos", "e" };
            string[] palavras = texto.ToLower().Split(' ');
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

            for (int i = 0; i < palavras.Length; i++)
            {
                string palavra = palavras[i];

                // Trata nomes compostos com hífen
                if (palavra.Contains("-"))
                {
                    string[] compostos = palavra.Split('-');
                    for (int j = 0; j < compostos.Length; j++)
                    {
                        compostos[j] = ti.ToTitleCase(compostos[j]);
                    }
                    palavra = string.Join("-", compostos);
                }
                else if (i == 0 || Array.IndexOf(excecoes, palavra) == -1)
                {
                    palavra = ti.ToTitleCase(palavra);
                }

                palavras[i] = palavra;
            }

            return string.Join(" ", palavras);
        }
    }
}
